using System;
using System.Collections.Generic;
using System.Threading;
using Common.DB;
using Moq;
using ProcessSystem.Contracts;
using ProcessSystem.DB;

namespace ProcessSystem.UnitTests
{
    public class RegisterRepositoryMock
    {
        private readonly Mock<IRegisterRepository> _registerRepositoryMock = new Mock<IRegisterRepository>();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        public static readonly string DefaultToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9" +
                                                     ".eyJzdWIiOiJ1c2VyIiwianRpIjoiMTYwZjU1NzUtNGIyNC00MzEzLWI3YzQtYzk1ZjNhYTk0YTQ5IiwiZXhwIjoxNjQ2MjA5MTM5LCJpc3MiOiJNVFMiLCJhdWQiOiJTaG9wV2luZG93In0" +
                                                     ".8BHzGfyC28wP_3u6W7UeT9_YvNMGLKokuH3n0Q_1g0g";
        public static readonly string DefaultUrl = "http://www.ourfirskmockwindow/mockcallback";
        public static readonly string DefaultName = "TestUser";
        public static readonly IList<string> DefaultProcessList = new List<string> { nameof(RegisterRepositoryMock) };

        #region SuccessSetups
        public void SetupAddSuccess()
        {
            _registerRepositoryMock.Setup(sw => sw.AddAsync(It.IsAny<Register>()))
                .ReturnsAsync((Register req) => req);
        }

        public void SetupDeleteSuccess(Register swr)
        {
            _registerRepositoryMock.Setup(sw => sw.DeleteAsync(It.Is<string>(s => s == swr.Token)))
                .ReturnsAsync(swr);
        }

        public void SetupFindByTokenSuccess(Register swr)
        {
            _registerRepositoryMock.Setup(sw => sw.FindByTokenAsync(It.Is<string>(s => s == swr.Token)))
                .ReturnsAsync(swr);
        }

        public void SetupFindByChannelAndUrlSuccess()
        {
            _registerRepositoryMock.Setup(sw => sw.FindByNameAndUrlAsync(It.IsAny<Register>()))
                .ReturnsAsync((Register req) => req);
        }

        public void SetupUnitOfWorkSuccess()
        {
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _unitOfWorkMock.Setup(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _registerRepositoryMock.SetupGet(sw => sw.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        #endregion

        #region FailureSetups
        public void SetupAddFailure()
        {
            _registerRepositoryMock.Setup(sw => sw.AddAsync(It.IsAny<Register>()))
                .ReturnsAsync((Register)null);
        }

        public void SetupDeleteFailure()
        {
            _registerRepositoryMock.Setup(sw => sw.DeleteAsync(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentNullException(nameof(Register), "По токену не найдена запись"));
        }

        public void SetupFindByTokenFailure()
        {
            _registerRepositoryMock.Setup(sw => sw.FindByTokenAsync(It.IsAny<string>()))
                .ReturnsAsync((Register)null);
        }

        public void SetupFindByChannelAndUrlFailure()
        {
            _registerRepositoryMock.Setup(sw => sw.FindByNameAndUrlAsync(It.IsAny<Register>()))
                .ReturnsAsync((Register)null);
        }

        public void SetupUnitOfWorkFailure()
        {
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            _unitOfWorkMock.Setup(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _registerRepositoryMock.SetupGet(sw => sw.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        #endregion
        
        /// <summary>
        /// Возвращает сам заглушенный объект 
        /// </summary>
        /// <returns></returns>
        public IRegisterRepository GetMockObject()
        {
            return _registerRepositoryMock.Object;
        }
    }
}
