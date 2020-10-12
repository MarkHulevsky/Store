using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Users;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters;

namespace Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers
{
    public static class UserResponseMapper
    {
        private static readonly Mapper<User, UserModel> _userModelMapper;
        static UserResponseMapper()
        {
            _userModelMapper = new Mapper<User, UserModel>();
        }
        public static UserResponseModel Map(UserResponseDataModel responseFilter)
        {
            if (responseFilter is null)
            {
                return new UserResponseModel();
            }

            var responseFilterModel = new UserResponseModel
            {
                TotalCount = responseFilter.TotalCount
            };

            foreach (var user in responseFilter.Users)
            {
                var userModel = _userModelMapper.Map(user);
                responseFilterModel.Users.Add(userModel);
            }
            return responseFilterModel;
        }
    }
}
