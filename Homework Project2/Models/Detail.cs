//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Homework_Project2.Models
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Detail
    {
        public int Id { get; set; }

        [DisplayName("帳號")]
        public string OrderGuid { get; set; }
        [DisplayName("會員帳號")]
        public string UserId { get; set; }
        [DisplayName("產品編號")]
        public string PId { get; set; }
        [DisplayName("品名")]
        public string Name { get; set; }
        [DisplayName("價格")]
        public Nullable<int> Price { get; set; }
        [DisplayName("訂購數量")]
        public Nullable<int> Qty { get; set; }
        [DisplayName("是否為訂單")]
        public string IsApproved { get; set; }
    }
}
