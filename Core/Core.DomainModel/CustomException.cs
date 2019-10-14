using System;
using System.Data.SqlClient;

namespace Core.DomainModel
{
    public enum ExceptionKey
    {
        NotDefined = -1,

        TimeoutExpired = -2,
        HasForeignKey = 547,
        HasDuplicateInfo = 2601,
        KeyAlreadyExsits = 2627,
        ArithmeticOverflow = 8115,

        NoActiveTransaction,
        InvalidOrderID,
        OrderItemsNotExist,
        InvalidOrderItemsProductType,
        InvalidOrderItemsQuantity
    }

    public class CustomException : Exception
    {

        #region Properties

        public ExceptionKey ExceptionKey { get; private set; }

        public string CustomMessage { get; private set; }

        #endregion /Properties

        #region Constructors

        public CustomException(Exception exception)
        {
            Exception innerException = null;
            if (exception.InnerException != null)
            {
                innerException = exception.InnerException;
            }
            else
            {
                innerException = exception;
            }
            this.CustomMessage = GetMessage(innerException);
        }

        public CustomException(ExceptionKey exceptionKey, params object[] args)
        {
            this.ExceptionKey = exceptionKey;
            this.CustomMessage = string.Format(GetMessage(null), args);
        }

        public CustomException(string message)
        {
            this.ExceptionKey = ExceptionKey.NotDefined;
            this.CustomMessage = message;
        }

        #endregion /Constructors

        #region Methods

        private string GetMessage(Exception innerException)
        {
            string result = string.Empty;

            if (innerException != null)
            {
                result = innerException.Message;

                if (innerException.GetBaseException() is SqlException)
                {
                    this.ExceptionKey = (ExceptionKey)(innerException.GetBaseException() as SqlException).Number;
                }

                switch (this.ExceptionKey)
                {
                    case ExceptionKey.TimeoutExpired:
                        result = Constant.Exception_sql_TimeoutExpired;
                        break;
                    case ExceptionKey.HasForeignKey:
                        result = Constant.Exception_sql_HasDepandantInfo;
                        break;
                    case ExceptionKey.HasDuplicateInfo:
                        result = Constant.Exception_sql_HasDuplicateInfo;
                        break;
                    case ExceptionKey.KeyAlreadyExsits:
                        result = Constant.Exception_sql_KeyAlreadyExsits;
                        break;
                    case ExceptionKey.ArithmeticOverflow:
                        result = Constant.Exception_sql_ArithmeticOverflow;
                        break;
                    default:
                        result = Constant.Exception_HasError;
                        break;
                }
            }
            else
            {
                switch (this.ExceptionKey)
                {
                    case ExceptionKey.NoActiveTransaction:
                        result = Constant.Exception_NoActiveTransaction;
                        break;
                    case ExceptionKey.InvalidOrderID:
                        result = Constant.Exception_InvalidOrderID;
                        break;
                    case ExceptionKey.OrderItemsNotExist:
                        result = Constant.Exception_OrderItemsNotExist;
                        break;
                    case ExceptionKey.InvalidOrderItemsProductType:
                        result = Constant.Exception_InvalidOrderItemsProductType;
                        break;
                    case ExceptionKey.InvalidOrderItemsQuantity:
                        result = Constant.Exception_InvalidOrderItemsQuantity;
                        break;
                    case ExceptionKey.NotDefined:
                    default:
                        result = Constant.Exception_HasError;
                        break;
                }
            }

            return result;
        }

        #endregion /Methods

    }
}