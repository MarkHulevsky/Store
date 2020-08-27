using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Users;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters;

namespace Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers
{
    public static class UserResponseFilterMapper
    {
        private static readonly Mapper<User, UserModel> _userModelMapper = new Mapper<User, UserModel>();
        public static UserResponseModel Map(UserResponseDataModel responseFilter)
        {
            var responseFilterModel = new UserResponseModel
            {
                TotalCount = responseFilter.TotalCount
            };

            foreach (var user in responseFilter.Users)
            {
                var userModel = _userModelMapper.Map(new UserModel(), user);
                responseFilterModel.Users.Add(userModel);
            }
            return responseFilterModel;
        }
    }
}
