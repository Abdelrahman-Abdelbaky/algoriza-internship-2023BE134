namespace VezeetaProject.Services
{
    public class LanguageConverterServices
    {
        public static string WeeKDaysFromEnglishToArabic (int select){

            var day = select switch
            {
                0=> "السبت",
                1=> "الاحد",
                2=> "الاثنين",
                3=> "الثلاثاء",
                4=> "الاربعاء",
                5=> "الخميس",
                6=> "الجمعة",
            };

            return day.ToString ();
        }

        public static string GenderFromEnglishToArabic (int select){

            var day = select switch
            {
                0=> "أنثى",
                1=> "ذكر"
            };

            return day.ToString ();

        }


        public static string StatusFromEnglishToArabic(int select)
        {

            var day = select switch
            {
                0 => "قيد الانتظار",
                1 => "مكتمل",
                2 => "الغيت"
            };

            return day.ToString();

        }
    }
}
