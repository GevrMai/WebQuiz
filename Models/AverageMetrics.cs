namespace WebQuiz.Models
{
    public class AverageMetrics
    {
        public static float AverageCorrectAnswers { get; set; }
        public static string AverageTime { get; set; }
        public static double AveragePoints { get; set; }

        public static float AverageUserCorrectAnswers { get; set; }
        public static string AverageUserTime { get; set; }
        public static double AverageUserPoints { get; set; }

        public async static Task CountMetricsAsync(int type, List<Result>? statistics)
        {
            await Task.Run(() => CountMetrics(type, statistics));
        }
        private static void CountMetrics(int type, List<Result>? statistics)
        {
            int sumCorrectAnswers = 0;
            int sumSeconds = 0;
            double sumPoints = 0.0;
            foreach (var result in statistics)
            {
                sumCorrectAnswers += result.CorrectAnswers;

                var timeComponents = result.Time.Split(":");
                sumSeconds += Convert.ToInt32(timeComponents[0]) * 60 + Convert.ToInt32(timeComponents[1]);

                sumPoints += result.Points;
            }
            sumSeconds = (int)Math.Round(sumSeconds / (double)statistics.Count());           // среднее значение
            //sumSeconds = (int)(Math.Round((double)sumSeconds / (double)statistics.Count()));           // среднее значение

            if (type == 0)
            {
                AverageCorrectAnswers = (float)Math.Round((float)sumCorrectAnswers / statistics.Count, 2);      // среднее кол-во верных ответов у всех
                AverageTime = (sumSeconds / 60).ToString() + ":" + (sumSeconds - ((sumSeconds / 60) * 60)).ToString();   //преобразование секунд в формат "мм:сс"
                AveragePoints = Math.Round(sumPoints / statistics.Count, 2);
            }
            else if (type == 1)
            {
                AverageUserCorrectAnswers = (float)Math.Round((float)sumCorrectAnswers / statistics.Count, 2);      // среднее кол-во верных ответов у пользователя
                AverageUserTime = (sumSeconds / 60).ToString() + ":" + (sumSeconds - ((sumSeconds / 60) * 60)).ToString();   //преобразование секунд в формат "мм:сс"
                AverageUserPoints = Math.Round(sumPoints / statistics.Count, 2);
            }
        }
    }
}
