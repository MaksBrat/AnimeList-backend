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
using System.Diagnostics;
using AnimeList.Domain.Enum;
using AnimeList.Common.Utitlities;
using AnimeList.Common.Extentions;

namespace AnimeList.Services.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProfileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IBaseResponse<UserProfileResponseModel> Edit(ProfileRequestModel model, int userId)
        {
            var profile = _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefault(
            predicate: x => x.UserId == userId);

            if (profile == null)
            {
                return new BaseResponse<UserProfileResponseModel>
                {
                    Description = "Profile not found"
                };
            }

            profile.Age = model.Age;
            profile.Name = model.Name;

            var response = new UserProfileResponseModel
            {
                Age = profile.Age,
                Name = profile.Name,
                RegistratedAt = profile.RegistratedAt,
                Avatar = profile.Avatar
            }; 

            _unitOfWork.GetRepository<UserProfile>().Update(profile);
            _unitOfWork.SaveChanges();

            return new BaseResponse<UserProfileResponseModel>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK,
            };
        }
        public IBaseResponse<UserProfileResponseModel> ChangeAvatar(IFormFile avatar, int userId)
        {
            var profile = _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefault(
            predicate: x => x.UserId == userId);

            if (profile == null)
            {
                return new BaseResponse<UserProfileResponseModel>
                {
                    Description = "Profile not found"
                };
            }

            if (avatar != null)
            {
                profile.Avatar = ImageConverter.ImageToByteArray(avatar);
            }

            _unitOfWork.GetRepository<UserProfile>().Update(profile);
            _unitOfWork.SaveChanges();

            var response = new UserProfileResponseModel
            {
                Age = profile.Age,
                Name = profile.Name,
                RegistratedAt = profile.RegistratedAt,
                Avatar = profile.Avatar
            };

            return new BaseResponse<UserProfileResponseModel>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK,
            };
        }
        public async Task<IBaseResponse<UserProfile>> Create(ApplicationUser user)
        {
            var profile = await _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefaultAsync(
                predicate: x => x.Id == user.Id);

            if(profile != null)
            {
                return new BaseResponse<UserProfile>
                {
                    Description = "Profile already exists",
                    StatusCode = HttpStatusCode.OK
                };
            }
           
            profile = new UserProfile
            {
                Name = $"User{UserIdExtensions.GetId:00000000}",
                Age = 100,
                RegistratedAt = DateTime.Now,
                UserId = user.Id,
                Avatar = ImageConverter.SetDefaultImage("user-default-image.png")
            };

            _unitOfWork.GetRepository<UserProfile>().Insert(profile);
            _unitOfWork.SaveChanges();

            return new BaseResponse<UserProfile>
            {
                Data = profile,
                StatusCode = HttpStatusCode.OK
            };
        }
        public async Task<IBaseResponse<UserProfileResponseModel>> Get(int UserId)
        {   
            try
            {   
                var profile = await _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefaultAsync(
                    predicate: x => x.UserId == UserId,
                    selector: x => new UserProfileResponseModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Age = x.Age,
                            Avatar = x.Avatar,
                            RegistratedAt = x.RegistratedAt
                        }
                 )!;

                return new BaseResponse<UserProfileResponseModel>()
                {
                    Data = profile,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserProfileResponseModel>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Description = $"Внутрішня помилка: {ex.Message}"
                };
            }
        }
        public IBaseResponse<ProfileAnimeListResponseModel> GetProfileWithAnimeList(int userId)
        {
            try
            {
                var profile = _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefault(
                    predicate: x => x.UserId == userId,
                    include: i => i
                        .Include(x => x.AnimeList)
                            .ThenInclude(x => x.Anime)
                                   .ThenInclude(x => x.AnimeGenres)
                                       .ThenInclude(x => x.Genre));

                var profileResponse = _mapper.Map<UserProfileResponseModel>(profile);
                var animeListResponse = _mapper.Map<ICollection<UserAnimeListResponseModel>>(profile.AnimeList);
                return new BaseResponse<ProfileAnimeListResponseModel>()
                {
                    Data = new ProfileAnimeListResponseModel { Profile = profileResponse, AnimeList = animeListResponse },
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ProfileAnimeListResponseModel>()
                {
                    Description = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<bool> AddAnimeToList(int userId, int animeId)
        {
            try
            {
                var userAnime = new UserAnimeList
                {
                    AnimeId = animeId,
                    ProfileId = userId,
                    AnimeStatus = Domain.Enum.AnimeStatus.WantToWatch
                };

                _unitOfWork.GetRepository<UserAnimeList>().Insert(userAnime);
                _unitOfWork.SaveChanges();

                return new BaseResponse<bool>
                {
                    Data = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<bool> DeleteAnimeFromList(int animeId, int userId)
        {
            try
            {
                var id = _unitOfWork.GetRepository<UserAnimeList>().GetFirstOrDefault(
                    predicate: x => x.Anime.Id == animeId && x.ProfileId == userId).Id;

                _unitOfWork.GetRepository<UserAnimeList>().Delete(id);
                _unitOfWork.SaveChanges();
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<bool> ChangeUserRating (int id, int rating)
        {
            try
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
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }       
        }
        public IBaseResponse<bool> ChangeWatchedEpisodes(int id, int episodes)
        {
            try
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
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<bool> ChangeAnimeStatus(int id, string status)
        {
            try
            {
                var animeList = _unitOfWork.GetRepository<UserAnimeList>().GetFirstOrDefault(
                predicate: x => x.Id == id);

                animeList.AnimeStatus = Enum.Parse<AnimeStatus>(status);
                _unitOfWork.GetRepository<UserAnimeList>().Update(animeList);
                _unitOfWork.SaveChanges();

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }            
        }
    }
}
