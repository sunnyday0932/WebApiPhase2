namespace WebApiPhase2Service.Dtos
{
    public class AccountDto
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 異動日期
        /// </summary>
        public string ModifyDate { get; set; }

        /// <summary>
        /// 異動人員
        /// </summary>
        public string ModifyUser { get; set; }
    }
}