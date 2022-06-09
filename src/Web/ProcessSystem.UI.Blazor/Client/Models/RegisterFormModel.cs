using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProcessSystem.UI.Blazor.Client.Validators;

namespace ProcessSystem.UI.Blazor.Client.Models
{
    public class RegisterFormModel
    {
        [Required(ErrorMessage = "Поле Название должно быть заполнено")]
        [StringLength(20, ErrorMessage = "Название слишком длинное")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле Ссылка на callback должно быть заполнено")]
        [Url(ErrorMessage = "Поле «URL» не является действительным полным URL-адресом http, https или ftp.")]
        public string Url { get; set; }

        [Required] //нужно, но будет потом как то заполнять
        [CheckEmptyCollection(ErrorMessage = "Необходим хотя бы один тип бизнес процесса")]
        public List<string> ProcessTypesList { get; set; }
    }
}