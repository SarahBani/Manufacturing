namespace Core.DomainModel
{
    public static class Constant
    {

        #region AppSetting

        public const string AppSetting_DefaultConnection = "DefaultConnection";

        #endregion /AppSetting

        #region Exceptions

        public const string Exception_HasError = "An error has occured!";

        public const string Exception_NoActiveTransaction = "There is no active transation!";        
        public const string Exception_InvalidOrderID = "OrderID is invalid!";
        public const string Exception_OrderItemsNotExist = "There is not any order item!";
        public const string Exception_InvalidOrderItemsProductType = "Product types in order items are invalid!";
        public const string Exception_InvalidOrderItemsQuantity = "Quantities in order items are invalid!";
        public const string Exception_OrderNotExist = "The order does not exists!";
        public const string Exception_InvalidOrder = "The order is invalid!";

        public const string Exception_sql_TimeoutExpired = "Database timeout has expired!";
        public const string Exception_sql_HasDepandantInfo = "The record has depandant information!";
        public const string Exception_sql_HasDuplicateInfo = "The record has duplicate information!";
        public const string Exception_sql_KeyAlreadyExsits = "The record key already exists!";        
        public const string Exception_sql_ArithmeticOverflow = "The record field value is too big!";

        #endregion /Exceptions

        public const string RequiredBinWidth = "{0} mm";

    }
}
