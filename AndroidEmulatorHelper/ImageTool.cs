using OpenCvSharp;

namespace AndroidEmulatorHelper
{
    public class ImageTool
    {
        public static SimilarRes Find(Mat origin, Mat find)
        {
            var res = origin.MatchTemplate(find, TemplateMatchModes.CCoeffNormed);
            Cv2.MinMaxLoc(res, out _, out double max, out _, out Point maxLoc);

            return new SimilarRes(max, maxLoc.X, maxLoc.Y);
        }

        public class SimilarRes
        {
            public double similar;

            public int startX;
            public int startY;

            public SimilarRes(double similar, int startX, int startY)
            {
                this.similar = similar;
                this.startX = startX;
                this.startY = startY;
            }
        }
    }
}
