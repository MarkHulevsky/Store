using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogicLayer.Models.Users;
using Store.DataAccess.Filters;
using Store.DataAccessLayer.Entities;

namespace Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers
{
    public static class UserResponseFilterMapper
    {
        private static readonly Mapper<User, UserModel> _userModelMapper = new Mapper<User, UserModel>();
        public static UserResponseFilterModel Map(UserResponseFilter responseFilter)
        {
            var responseFilterModel = new UserResponseFilterModel();
            responseFilterModel.TotalCount = responseFilter.TotalCount;

            foreach (var user in responseFilter.Users)
            {
                var userModel = _userModelMapper.Map(new UserModel(), user);
                responseFilterModel.Users.Add(userModel);
            }
            return responseFilterModel;
        }
    }
}
