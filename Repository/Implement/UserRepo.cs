using AutoMapper;
using BusinessObject.Model;
using DAO.Helper;
using DAO.UnitOfWork;
using DTO.UserDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Linq.Expressions;
using System.Reflection;

namespace Repository.Implement
{
    public class UserRepo : IUserRepo
    {
        private readonly UserManager<User> _userManager;
        private readonly UnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IValidateGet _validateGet;

        public UserRepo(UserManager<User> userManager, IMapper mapper, IValidateGet validateGet)
        {
            _userManager = userManager;
            _unitOfWork = new UnitOfWork();
            _mapper = mapper;
            _validateGet = validateGet;
        }

        public async Task<OperationResult<UserResponseDTO>> Login(UserLogin resource)
        {
            var result = new OperationResult<UserResponseDTO>()
            {
                IsError = false
            };

            var existedUser = await _userManager.FindByNameAsync(resource.UserName);
            if (existedUser is null || existedUser.Deleted)
            {
                result.AddError(ErrorCode.BadRequest, "Invalid username or password");
                return result;
            }

            var checkPassword = await _userManager.CheckPasswordAsync(existedUser, resource.Password);
            if (!checkPassword)
            {
                result.AddError(ErrorCode.BadRequest, "Invalid username or password");
                return result;
            }

            if (existedUser.LockoutEnabled)
            {
                result.AddError(ErrorCode.BadRequest, "Account is blocked");
                return result;
            }
            else
            {
                result.Payload = _mapper.Map<UserResponseDTO>(existedUser);
                return result;
            }

        }

        public async Task<OperationResult<UserResponseDTO>> Register(UserDTO resource)
        {
            var result = new OperationResult<UserResponseDTO>()
            {
                IsError = false
            };

            if (resource.UserName is null || resource.Password is null || resource.FirstName is null || resource.LastName is null)
            {
                result.AddError(ErrorCode.BadRequest, "Invalid input");
                return result;
            }

            var existedUser = await _userManager.FindByNameAsync(resource.UserName);
            if (existedUser is not null)
            {
                result.AddError(ErrorCode.BadRequest, "Username is already taken");
                return result;
            }

            if (resource.Email is not null)
            {
                var existedEmail = await _unitOfWork.UserDAO.Get(l => resource.Email.Equals(l.Email)).FirstOrDefaultAsync();
                if (existedEmail is not null)
                {
                    result.AddError(ErrorCode.BadRequest, "Email is already taken");
                    return result;
                }
            }

            if (resource.PhoneNumber is not null)
            {
                var existedPhone = await _unitOfWork.UserDAO.Get(l => resource.PhoneNumber.Equals(l.PhoneNumber)).FirstOrDefaultAsync();
                if (existedPhone is not null)
                {
                    result.AddError(ErrorCode.BadRequest, "Phone is already taken");
                    return result;
                }
            }

            if (!resource.Password.Equals(resource.ConfirmPassword))
            {
                result.AddError(ErrorCode.BadRequest, "Password and confirm password not match");
                return result;
            }

            try
            {
                User newUser = _mapper.Map<User>(resource);
                var identityResult = await _userManager.CreateAsync(newUser, resource.Password);
                if (!identityResult.Succeeded)
                {
                    foreach (var error in identityResult.Errors)
                    {
                        result.AddError(ErrorCode.BadRequest, error.Description);
                    }
                    return result;
                }
                else
                {
                    result.Payload = _mapper.Map<UserResponseDTO>(newUser);
                    return result;
                }
            }
            catch (DbUpdateException ex)
            {
                result.AddError(ErrorCode.BadRequest, ex.Message);
                return result;
            }
            catch (OperationCanceledException ex)
            {
                result.AddError(ErrorCode.BadRequest, "The operation has been cancelled");
                result.AddError(ErrorCode.BadRequest, ex.Message);
                return result;
            }
        }

        public async Task<OperationResult<UserResponseDTO>> Update(UserDTO resource, Guid id)
        {
            var result = new OperationResult<UserResponseDTO>()
            {
                IsError = false
            };

            var existedUser = await _unitOfWork.UserDAO.Get(u => u.Id == id && !u.Deleted).FirstOrDefaultAsync();
            if (existedUser is null)
            {
                result.AddError(ErrorCode.BadRequest, "User not found");
                return result;
            }

            if (resource.Email is not null)
            {
                var existedEmail = await _unitOfWork.UserDAO.Get(u => u.Email.Equals(resource.Email) && u.Id != id).AsNoTracking().FirstOrDefaultAsync();
                if (existedEmail is not null)
                {
                    result.AddError(ErrorCode.BadRequest, "Email is already taken");
                    return result;
                }
                existedUser.Email = resource.Email;
                existedUser.NormalizedEmail = resource.Email.ToUpper();
            }

            if (resource.PhoneNumber is not null)
            {
                var existedPhone = await _unitOfWork.UserDAO.Get(u => u.PhoneNumber.Equals(resource.PhoneNumber) && u.Id != id).AsNoTracking().FirstOrDefaultAsync();
                if (existedPhone is not null)
                {
                    result.AddError(ErrorCode.BadRequest, "Phone is already taken");
                    return result;
                }
                existedUser.PhoneNumber = resource.PhoneNumber;
            }

            if (resource.CoffeeShopId is not null)
            {
                if (resource.CoffeeShopId <= 0)
                {
                    existedUser.CoffeeShopId = null;
                }
                else
                {
                    var existedCoffeeShop = await _unitOfWork.CoffeeShopDAO.Get(c => c.CoffeeShopId == resource.CoffeeShopId).AsNoTracking().FirstOrDefaultAsync();
                    if (existedCoffeeShop is null)
                    {
                        result.AddError(ErrorCode.BadRequest, "Coffee Shop not found");
                        return result;
                    }
                    existedUser.CoffeeShopId = resource.CoffeeShopId;
                }
            }

            if (resource.ManagerShopId is not null)
            {
                if (resource.ManagerShopId <= 0)
                {
                    existedUser.ManagerShopId = null;
                }
                else
                {
                    var existedCoffeeShop = await _unitOfWork.CoffeeShopDAO.Get(c => c.CoffeeShopId.Equals(resource.ManagerShopId)).AsNoTracking().FirstOrDefaultAsync();
                    if (existedCoffeeShop is not null)
                    {
                        result.AddError(ErrorCode.BadRequest, "Coffee Shop not found");
                        return result;
                    }
                    existedUser.ManagerShopId = resource.CoffeeShopId;
                }
            }

            existedUser.FirstName = resource.FirstName ?? existedUser.FirstName;
            existedUser.LastName = resource.LastName ?? existedUser.LastName;
            existedUser.LockoutEnd = resource.LockoutEnd ?? existedUser.LockoutEnd;
            existedUser.LockoutEnabled = resource.LockoutEnabled ?? existedUser.LockoutEnabled;
            existedUser.Role = resource.Role ?? existedUser.Role;
            existedUser.Status = resource.Status ?? existedUser.Status;

            if (resource.Password is not null)
            {
                if (!resource.Password.Equals(resource.ConfirmPassword))
                {
                    result.AddError(ErrorCode.BadRequest, "Password and confirm password not match");
                    return result;
                }
                var identityResult = await _userManager.ChangePasswordAsync(existedUser, resource.OldPassword, resource.Password);
                if (identityResult.Succeeded)
                {
                    result.Payload = _mapper.Map<UserResponseDTO>(existedUser);
                    return result;
                }
                else
                {
                    foreach (var error in identityResult.Errors)
                    {
                        result.AddError(ErrorCode.BadRequest, error.Description);
                        return result;
                    }
                }
            }

            try
            {
                var updateResult = await _unitOfWork.SaveChangesAsync();
                if (updateResult)
                {
                    result.Payload = _mapper.Map<UserResponseDTO>(existedUser);
                    return result;
                }
                else
                {
                    result.AddError(ErrorCode.BadRequest, "Update failed");
                    return result;
                }
            }
            catch (DbUpdateException ex)
            {
                result.AddError(ErrorCode.BadRequest, ex.Message);
                return result;
            }
            catch (OperationCanceledException ex)
            {
                result.AddError(ErrorCode.BadRequest, "The operation has been cancelled");
                result.AddError(ErrorCode.BadRequest, ex.Message);
                return result;
            }
        }

        public async Task<OperationResult<object>> Delete(Guid id)
        {
            var result = new OperationResult<object>()
            {
                IsError = false
            };

            var existedUser = await _unitOfWork.UserDAO.Get(u => u.Id == id && !u.Deleted).FirstOrDefaultAsync();
            if (existedUser is null)
            {
                result.AddError(ErrorCode.BadRequest, "User not found");
                return result;
            }

            existedUser.Deleted = true;

            try
            {
                var updateResult = await _unitOfWork.SaveChangesAsync();
                if (updateResult)
                {
                    return result;
                }
                else
                {
                    result.AddError(ErrorCode.BadRequest, "Delete failed");
                    return result;
                }
            }
            catch (DbUpdateException ex)
            {
                result.AddError(ErrorCode.BadRequest, ex.Message);
                return result;
            }
            catch (OperationCanceledException ex)
            {
                result.AddError(ErrorCode.BadRequest, "The operation has been cancelled");
                result.AddError(ErrorCode.BadRequest, ex.Message);
                return result;
            }
        }

        public async Task<OperationResult<UserResponseDTO>> GetById(Guid id)
        {
            var result = new OperationResult<UserResponseDTO>()
            {
                IsError = false
            };

            var existedUser = await _unitOfWork.UserDAO.Get(u => u.Id == id && !u.Deleted).FirstOrDefaultAsync();
            if (existedUser is null)
            {
                result.AddError(ErrorCode.NotFound, "User not found");
                return result;
            }

            result.Payload = _mapper.Map<UserResponseDTO>(existedUser);
            return result;
        }

        public async Task<OperationResult<IEnumerable<UserResponseDTO>>> Get(int startPage, int endPage, int? quantity, string? name, byte? role, byte? status, int? coffeeShopId, int? managedShopId)
        {
            var result = new OperationResult<IEnumerable<UserResponseDTO>>()
            {
                IsError = false
            };

            int quantityResult = 0;
            _validateGet.ValidateGetRequest(ref startPage, ref endPage, quantity, ref quantityResult);
            if (quantityResult == 0)
            {
                result.AddError(ErrorCode.BadRequest, "Invalid get quantity");
                return result;
            }

            var expressions = new List<Expression>();
            ParameterExpression pe = Expression.Parameter(typeof(User), "r");
            MethodInfo? containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            if (containsMethod is null)
            {
                result.AddError(ErrorCode.ServerError, "Method Contains can not found from string type");
                return result;
            }

            expressions.Add(Expression.Equal(Expression.Property(pe, nameof(User.Deleted)), Expression.Constant(false)));

            if (name is not null)
            {
                expressions.Add(
                    Expression.Or(
                        Expression.Call(Expression.Property(pe, nameof(User.FirstName)), containsMethod, Expression.Constant(name)),
                        Expression.Call(Expression.Property(pe, nameof(User.LastName)), containsMethod, Expression.Constant(name))
                    ));
            }

            if (role is not null)
            {
                expressions.Add(Expression.Equal(Expression.Property(pe, nameof(User.Role)), Expression.Constant(role)));
            }

            if (status is not null)
            {
                expressions.Add(Expression.Equal(Expression.Property(pe, nameof(User.Status)), Expression.Constant(status)));
            }

            if (coffeeShopId is not null)
            {
                expressions.Add(Expression.Equal(Expression.Property(pe, nameof(User.CoffeeShopId)), Expression.Constant(coffeeShopId)));
            }

            if (managedShopId is not null)
            {
                expressions.Add(Expression.Equal(Expression.Property(pe, nameof(User.ManagerShopId)), Expression.Constant(managedShopId)));
            }

            Expression combined = expressions.Aggregate((accumulate, next) => Expression.AndAlso(accumulate, next));
            Expression<Func<User, bool>> where = Expression.Lambda<Func<User, bool>>(combined, pe);

            var users = await _unitOfWork.UserDAO
                .Get(where)
                .AsNoTracking()
                .Skip((startPage - 1) * quantityResult)
                .Take((endPage - startPage + 1) * quantityResult)
                .ToArrayAsync();

            result.Payload = _mapper.Map<IEnumerable<UserResponseDTO>>(users);
            return result;
        }
    }
}
