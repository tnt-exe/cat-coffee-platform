using BusinessObject.Model;
using DAO.Context;
using DAO.Generic;

namespace DAO.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        #region Area
        private BaseDAO<Area> _areaDAO;

        public BaseDAO<Area> AreaDAO
        {
            get
            {
                if (_areaDAO == null)
                {
                    _areaDAO = new BaseDAO<Area>(_context);
                }
                return _areaDAO;
            }
        }
        #endregion

        #region Bill
        private BaseDAO<Bill> _billDAO;

        public BaseDAO<Bill> BillDAO
        {
            get
            {
                if (_billDAO == null)
                {
                    _billDAO = new BaseDAO<Bill>(_context);
                }
                return _billDAO;
            }
        }
        #endregion

        #region Booking
        private BaseDAO<Booking> _bookingDAO;

        public BaseDAO<Booking> BookingDAO
        {
            get
            {
                if (_bookingDAO == null)
                {
                    _bookingDAO = new BaseDAO<Booking>(_context);
                }
                return _bookingDAO;
            }
        }
        #endregion

        #region BookingProduct
        private BaseDAO<BookingProduct> _bookingProductDAO;

        public BaseDAO<BookingProduct> BookingProductDAO
        {
            get
            {
                if (_bookingProductDAO == null)
                {
                    _bookingProductDAO = new BaseDAO<BookingProduct>(_context);
                }
                return _bookingProductDAO;
            }
        }
        #endregion

        #region Cat
        private BaseDAO<Cat> _catDAO;

        public BaseDAO<Cat> CatDAO
        {
            get
            {
                if (_catDAO == null)
                {
                    _catDAO = new BaseDAO<Cat>(_context);
                }
                return _catDAO;
            }
        }
        #endregion

        #region Category
        private BaseDAO<Category> _categoryDAO;

        public BaseDAO<Category> CategoryDAO
        {
            get
            {
                if (_categoryDAO == null)
                {
                    _categoryDAO = new BaseDAO<Category>(_context);
                }
                return _categoryDAO;
            }
        }
        #endregion

        #region CoffeeShop
        private BaseDAO<CoffeeShop> _coffeeShopDAO;

        public BaseDAO<CoffeeShop> CoffeeShopDAO
        {
            get
            {
                if (_coffeeShopDAO == null)
                {
                    _coffeeShopDAO = new BaseDAO<CoffeeShop>(_context);
                }
                return _coffeeShopDAO;
            }
        }
        #endregion

        #region Payment
        private BaseDAO<Payment> _paymentDAO;

        public BaseDAO<Payment> PaymentDAO
        {
            get
            {
                if (_paymentDAO == null)
                {
                    _paymentDAO = new BaseDAO<Payment>(_context);
                }
                return _paymentDAO;
            }
        }
        #endregion

        #region Product
        private BaseDAO<Product> _productDAO;

        public BaseDAO<Product> ProductDAO
        {
            get
            {
                if (_productDAO == null)
                {
                    _productDAO = new BaseDAO<Product>(_context);
                }
                return _productDAO;
            }
        }
        #endregion

        #region TimeFrame
        private BaseDAO<TimeFrame> _timeFrameDAO;

        public BaseDAO<TimeFrame> TimeFrameDAO
        {
            get
            {
                if (_timeFrameDAO == null)
                {
                    _timeFrameDAO = new BaseDAO<TimeFrame>(_context);
                }
                return _timeFrameDAO;
            }
        }
        #endregion

        #region User
        private BaseDAO<User> _userDAO;

        public BaseDAO<User> UserDAO
        {
            get
            {
                if (_userDAO == null)
                {
                    _userDAO = new BaseDAO<User>(_context);
                }
                return _userDAO;
            }
        }
        #endregion

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
