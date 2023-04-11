using System;
using System.Collections.Generic;
using FluentValidation;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories.ProductImageRepository.Constants
{
    public class ProductImageMessages
    {
        public static string Added = "Kayıt İşlemi Başarılı.!";
        public static string Updated = "Güncelleme İşlemi Başarılı.!";
        public static string Deleted = "Silme İşlemi Başarılı.!";
        public static string MainImageUpdate= "Seçtiğiniz Ürüne Ait Ana Resim Güncellendi.!";
    }
}
