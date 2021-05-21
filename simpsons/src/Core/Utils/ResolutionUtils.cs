namespace simpsons.Core.Utils
{
    public static class ResolutionUtils
    {
        public static int Width {get;set;}
        public static int Height {get;set;}
        
        public static float MidWidth {get;set;}


        public static void SetResolution(int width, int height)
        {
            Width = width;
            Height = height;

            MidWidth = ((float)Width / (float) 2);
        }
    }
}