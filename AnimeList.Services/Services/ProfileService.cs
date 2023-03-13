using AnimeList.DAL.Interfaces;
using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.RequestModels;
using AnimeList.Domain.Response;
using AnimeList.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Net;
using AnimeList.Domain.ResponseModels.Profile;
using AnimeList.Common.Extentions;
using AnimeList.Domain.Enums;
using AnimeList.Domain.Entity;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;

namespace AnimeList.Services.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public ProfileService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public IBaseResponse<UserProfileResponse> Edit(ProfileRequest model, int userId)
        {
            var profile = _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefault(
                predicate: x => x.UserId == userId,
                include: i => i
                    .Include(x => x.FileModel));

            if (profile == null)
            {
                return new BaseResponse<UserProfileResponse>
                {
                    Description = "Profile not found",
                    StatusCode = HttpStatusCode.Found,
                };
            }

            _mapper.Map(model, profile);

            _unitOfWork.GetRepository<UserProfile>().Update(profile);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<UserProfileResponse>(profile);

            return new BaseResponse<UserProfileResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK,
            };         
        }
        public async Task<IBaseResponse<UserProfileResponse>> ChangeAvatar(IFormFile avatar, int userId)
        {
            var profile = _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefault(
                predicate: x => x.UserId == userId,
                include: i => i
                    .Include(x => x.FileModel));

            if (profile == null)
            {
                return new BaseResponse<UserProfileResponse>
                {
                    Description = "Profile not found",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            var connectionString = _configuration["BlobStorage:BlobStorageConnection"];
            var containerName = _configuration["BlobStorage:ContainerName"];

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            
            var file = profile.FileModel;
            string oldPath = file.Path;

            BlobClient blobClient;

            if (oldPath != _configuration["BlobStorage:UserDefaultImageUrl"])
            {
                // Delete the old avatar image from blob storage
                blobClient = containerClient.GetBlobClient(file.Name);
                await blobClient.DeleteIfExistsAsync();
            }

            // Upload the new avatar image to blob storage
            var blobName = Guid.NewGuid().ToString() + Path.GetExtension(avatar.FileName);
            blobClient = containerClient.GetBlobClient(blobName);
            var result = await blobClient.UploadAsync(avatar.OpenReadStream());

            file.Path = blobClient.Uri.ToString();
            file.Name = blobName;

            _unitOfWork.GetRepository<FileModel>().Update(file);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<UserProfileResponse>(profile);

            return new BaseResponse<UserProfileResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK,
            };   
        }
        public async Task<IBaseResponse<UserProfileResponse>> Create(ApplicationUser user)
        {
            var profile = await _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefaultAsync(
            predicate: x => x.Id == user.Id);

            if (profile != null)
            {
                return new BaseResponse<UserProfileResponse>
                {
                    Description = "Profile already exists",
                    StatusCode = HttpStatusCode.OK
                };
            }

            var file = new FileModel { Name = "user-default-image.png", Path = _configuration["BlobStorage:UserDefaultImageUrl"] };

            _unitOfWork.GetRepository<FileModel>().Insert(file);
            _unitOfWork.SaveChanges();

            profile = new UserProfile
            {
                Name = $"User{UserIdExtensions.GetId:00000000}",
                RegistratedAt = DateTime.UtcNow,
                UserId = user.Id,
                FileModelId = file.Id
            };

            _unitOfWork.GetRepository<UserProfile>().Insert(profile);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<UserProfileResponse>(profile);

            return new BaseResponse<UserProfileResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };           
        }
        public async Task<IBaseResponse<UserProfileResponse>> Get(int UserId)
        {    
            var profile = await _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefaultAsync(
                predicate: x => x.UserId == UserId,
                include: i => i
                    .Include(x => x.FileModel));

            var response = _mapper.Map<UserProfileResponse>(profile);

            return new BaseResponse<UserProfileResponse>()
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };
        }
        public IBaseResponse<ProfileAnimeListResponse> GetProfileWithAnimeList(int userId)
        {
            var profile = _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefault(
                predicate: x => x.UserId == userId,
                include: i => i
                    .Include(x => x.AnimeList)
                        .ThenInclude(x => x.Anime)
                                .ThenInclude(x => x.AnimeGenres)
                                    .ThenInclude(x => x.Genre));

            var profileResponse = _mapper.Map<UserProfileResponse>(profile);
            var animeListResponse = _mapper.Map<ICollection<UserAnimeListResponse>>(profile.AnimeList);

            return new BaseResponse<ProfileAnimeListResponse>()
            {
                Data = new ProfileAnimeListResponse { Profile = profileResponse, AnimeList = animeListResponse },
                StatusCode = HttpStatusCode.OK
            };
        }
        public IBaseResponse<bool> AddAnimeToList(int userId, int animeId)
        {
            var userAnime = new UserAnimeList
            {
                AnimeId = animeId,
                ProfileId = userId,
                AnimeStatus = Domain.Enums.AnimeListStatus.WantToWatch
            };

            _unitOfWork.GetRepository<UserAnimeList>().Insert(userAnime);
            _unitOfWork.SaveChanges();

            return new BaseResponse<bool>
            {
                Data = true,
                StatusCode = HttpStatusCode.OK
            };
        }
        public IBaseResponse<bool> DeleteAnimeFromList(int animeId, int userId)
        {
            var animeInList = _unitOfWork.GetRepository<UserAnimeList>().GetFirstOrDefault(
                predicate: x => x.Anime.Id == animeId && x.ProfileId == userId);

            if(animeInList == null)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = "Anime in list is not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            _unitOfWork.GetRepository<UserAnimeList>().Delete(animeInList.Id);
            _unitOfWork.SaveChanges();

            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = HttpStatusCode.OK
            };
        }
        public IBaseResponse<bool> ChangeUserRating (int id, int? rating)
        {
            var animeList = _unitOfWork.GetRepository<UserAnimeList>().GetFirstOrDefault(
                predicate: x => x.Id == id);

            animeList.UserRating = rating;

            _unitOfWork.GetRepository<UserAnimeList>().Update(animeList);
            _unitOfWork.SaveChanges();

            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = HttpStatusCode.OK
            };      
        }
        public IBaseResponse<bool> ChangeWatchedEpisodes(int id, int? episodes)
        {
            var animeList = _unitOfWork.GetRepository<UserAnimeList>().GetFirstOrDefault(
                predicate: x => x.Id == id);

            animeList.WatchedEpisodes = episodes;

            _unitOfWork.GetRepository<UserAnimeList>().Update(animeList);
            _unitOfWork.SaveChanges();

            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = HttpStatusCode.OK
            };
        }
        public IBaseResponse<bool> ChangeAnimeStatus(int id, string status)
        {
            var animeList = _unitOfWork.GetRepository<UserAnimeList>().GetFirstOrDefault(
                predicate: x => x.Id == id);

            animeList.AnimeStatus = Enum.Parse<AnimeListStatus>(status);

            _unitOfWork.GetRepository<UserAnimeList>().Update(animeList);
            _unitOfWork.SaveChanges();

            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = HttpStatusCode.OK
            };           
        }
    }
}
