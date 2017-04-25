using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Bridge
{
    // This class holds the entry points to the C++ analysis library
    static class AnalysisBridge
    {

        public static bool DecodeFrames(String videoName, String outputDir, String extension)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return decodeFramesWin(videoName, outputDir, extension);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return decodeFramesLinux(videoName, outputDir, extension);
            }
            else
            {
                throw new System.ArgumentException("", "");
            }
        }

        // Main entry point for analysing a single shot.
        public static AnalysisResult AnalyzeShot(int firstFrame, int lastFrame,
                                                 Point2i[] targetCoords,
                                                 double sizeX, double sizeY,
                                                 Point2d[] targetOffsetsInCm,
                                                 String videoName)
        {
            if (targetCoords.Length != targetOffsetsInCm.Length)
            {
                throw new System.ArgumentException("targetOffsetsInCm.Length != targetCoords.Length",
                                                   "targetOffsetsInCm");
            }
            int[] targetCoordsFlat = new int[targetCoords.Length*2];
            double[] targetOffsetsInCmFlat = new double[targetOffsetsInCm.Length*2];
            int fi = 0;
            for (int pi = 0; pi < targetCoords.Length; ++pi)
            {
                targetCoordsFlat[fi] = targetCoords[pi].x;
                targetOffsetsInCmFlat[fi] = targetOffsetsInCm[pi].x;
                ++fi;
                targetCoordsFlat[fi] = targetCoords[pi].y;
                targetOffsetsInCmFlat[fi] = targetOffsetsInCm[pi].y;
                ++fi;
            }
            AnalysisResult ret = new AnalysisResult();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                analyzeShotCSWin(firstFrame, lastFrame, targetCoords.Length, targetCoordsFlat, sizeX, sizeY,
                            targetOffsetsInCmFlat, videoName, ret);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                analyzeShotCSLinux(firstFrame, lastFrame, targetCoords.Length, targetCoordsFlat, sizeX, sizeY,
                            targetOffsetsInCmFlat, videoName, ret);
            }
            else
            {
                throw new System.ArgumentException("", "");
            }
            return ret;
        }

        // Checks if a Perspective transform is rigid, ie no warping.
        // Hard to explain, see https://en.wikipedia.org/wiki/Rigid_transformation
        // not that the link will make it easier to understand though.
        public static bool IsRigidPerspectiveTransform(Point2d[] pointsSrcSpace,
                                                       Point2d[] pointsDstSpace)
        {
            if (pointsSrcSpace.Length != pointsDstSpace.Length)
            {
                return false;
            }
            double[] pointsSrcSpaceFlat = new double[pointsSrcSpace.Length*2];
            double[] pointsDstSpaceFlat = new double[pointsSrcSpace.Length*2];
            int fi = 0;
            for (int pi = 0; pi < pointsSrcSpace.Length; ++pi)
            {
                pointsSrcSpaceFlat[fi] = pointsSrcSpace[pi].x;
                pointsDstSpaceFlat[fi] = pointsDstSpace[pi].x;
                ++fi;
                pointsSrcSpaceFlat[fi] = pointsSrcSpace[pi].y;
                pointsDstSpaceFlat[fi] = pointsDstSpace[pi].x;
                ++fi;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return isRigidPerspectiveTransformCSWin(pointsSrcSpace.Length,
                                                        pointsSrcSpaceFlat,
                                                        pointsDstSpaceFlat);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return isRigidPerspectiveTransformCSLinux(pointsSrcSpace.Length,
                                                          pointsSrcSpaceFlat,
                                                          pointsDstSpaceFlat);
            }
            else
            {
                throw new System.ArgumentException("", "");
            }
        }

        // Does base conversion on srcSpacePoint from the base that pointsSrcSpace are in to
        // the one pointsDstSpace is in.
        // Required: pointsSrcSpace.Length == pointsDstSpace.Length
        // The main use case for this method for us (at least outside the analysis code) is
        // to convert from pixel space to cm space or the other way around.
        // For pixel->cm:
        // srcSpacePoint is the point we need to convert from pixel space to cm space.
        // pointsSrcSpace is the target points in pixels.
        // pointsDstSpace is the target points in cm.
        public static Point2d PointBaseChange(Point2d srcSpacePoint,
                                              Point2d[] pointsSrcSpace,
                                              Point2d[] pointsDstSpace)
        {
            if (pointsSrcSpace.Length != pointsDstSpace.Length)
            {
                throw new System.ArgumentException("pointsSrcSpace.Length != pointsDstSpace.Length",
                                                   "pointsDstSpace.Length");
            }
            double[] pointsSrcSpaceFlat = new double[pointsSrcSpace.Length*2];
            double[] pointsDstSpaceFlat = new double[pointsSrcSpace.Length*2];
            int fi = 0;
            for (int pi = 0; pi < pointsSrcSpace.Length; ++pi)
            {
                pointsSrcSpaceFlat[fi] = pointsSrcSpace[pi].x;
                pointsDstSpaceFlat[fi] = pointsDstSpace[pi].x;
                ++fi;
                pointsSrcSpaceFlat[fi] = pointsSrcSpace[pi].y;
                pointsDstSpaceFlat[fi] = pointsDstSpace[pi].x;
                ++fi;
            }
            DumPoint2d ret = new DumPoint2d();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                pointBaseChangeCSWin(srcSpacePoint.x, srcSpacePoint.y,
                                     pointsSrcSpace.Length,
                                     pointsSrcSpaceFlat,
                                     pointsDstSpaceFlat, ret);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                pointBaseChangeCSLinux(srcSpacePoint.x, srcSpacePoint.y,
                                     pointsSrcSpace.Length,
                                     pointsSrcSpaceFlat,
                                     pointsDstSpaceFlat, ret);
            }
            else
            {
                throw new System.ArgumentException("", "");
            }
            return new Point2d(ret.x, ret.y);
        }

        // Returns the vector pointing from cmTargetPoint to srcSpacePoint in cm with positive meaning up.
        public static Point2d SrcPointToCmVectorFromTargetPoint(Point2d srcSpacePoint,
                                                                Point2d cmTargetPoint,
                                                                Point2d[] pointsSrcSpace,
                                                                Point2d[] pointsCmSpace)
        {
            if (pointsSrcSpace.Length != pointsCmSpace.Length)
            {
                throw new System.ArgumentException("pointsSrcSpace.Length != pointsDstSpace.Length",
                                                   "pointsDstSpace.Length");
            }
            double[] pointsSrcSpaceFlat = new double[pointsSrcSpace.Length*2];
            double[] pointsDstSpaceFlat = new double[pointsCmSpace.Length*2];
            int fi = 0;
            for (int pi = 0; pi < pointsSrcSpace.Length; ++pi)
            {
                pointsSrcSpaceFlat[fi] = pointsSrcSpace[pi].x;
                pointsDstSpaceFlat[fi] = pointsCmSpace[pi].x;
                ++fi;
                pointsSrcSpaceFlat[fi] = pointsSrcSpace[pi].y;
                pointsDstSpaceFlat[fi] = pointsCmSpace[pi].x;
                ++fi;
            }
            DumPoint2d ret = new DumPoint2d();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                srcPointToCmVectorFromTargetPointCSWin(srcSpacePoint.x, srcSpacePoint.y,
                                                       cmTargetPoint.x, cmTargetPoint.y,
                                                       pointsSrcSpace.Length,
                                                       pointsSrcSpaceFlat, pointsDstSpaceFlat,
                                                       ret);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                srcPointToCmVectorFromTargetPointCSLinux(srcSpacePoint.x, srcSpacePoint.y,
                                                         cmTargetPoint.x, cmTargetPoint.y,
                                                         pointsSrcSpace.Length,
                                                         pointsSrcSpaceFlat, pointsDstSpaceFlat,
                                                         ret);
            }
            else
            {
                throw new System.ArgumentException("", "");
            }
            return new Point2d(ret.x, ret.y);
        }

        // Below are the actual entry points, duplicated since I dont like windows,
        // and this allows me to work (at least a little bit) in linux.

        private const String windowsSharedLibrary = "CppConversion.dll";
        private const String linuxSharedLibrary = "libanalyze.so";

        [DllImport(windowsSharedLibrary, EntryPoint = "decodeFrames")]
        private static extern bool decodeFramesWin(String videoName, String outputDir, String extension);

        //Windows entry points
        [DllImport(windowsSharedLibrary, EntryPoint = "analyzeShotCS")]
        private static extern void analyzeShotCSWin(int firstFrame, int lastFrame,
                                                    int targetNo,
                                                    [In] int[] targetCoords,
                                                    double sizeX, double sizeY,
                                                    [In] double[] targetOffsetsInCm,
                                                    String videoName,
                                                    [In, Out, MarshalAs(UnmanagedType.LPStruct)]
                                                    AnalysisResult ret);


        [DllImport(windowsSharedLibrary, EntryPoint = "isRigidPerspectiveTransformCS")]
        private static extern bool isRigidPerspectiveTransformCSWin(int len,
                                                                    [In] double[] pointsSrcSpaceFlat,
                                                                    [In] double[] pointsDstSpaceFlat);

        [DllImport(windowsSharedLibrary, EntryPoint = "pointBaseChangeCS")]
        private static extern void pointBaseChangeCSWin(double x, double y,
                                                        int len,
                                                        [In] double[] pointsSrcSpaceFlat,
                                                        [In] double[] pointsDstSpaceFlat,
                                                        [In, Out, MarshalAs(UnmanagedType.LPStruct)]
                                                        DumPoint2d ret);

        [DllImport(windowsSharedLibrary, EntryPoint = "srcPointToCmVectorFromTargetPointCS")]
        private static extern
            void srcPointToCmVectorFromTargetPointCSWin(double src_space_point_x,
                                                        double src_space_point_y,
                                                        double cm_target_point_x,
                                                        double cm_target_point_y,
                                                        int len,
                                                        [In]
                                                        double[] pointsSrcSpaceFlat,
                                                        [In]
                                                        double[] pointsDstSpaceFlat,
                                                        [In, Out,
                                                        MarshalAs(UnmanagedType.LPStruct)]
                                                        DumPoint2d ret);



        //Linux entry points


        [DllImport(linuxSharedLibrary, EntryPoint = "decodeFrames")]
        private static extern bool decodeFramesLinux(String videoName, String outputDir, String extension);

        [DllImport(linuxSharedLibrary, EntryPoint = "analyzeShotCS")]
        private static extern void analyzeShotCSLinux(int firstFrame, int lastFrame,
                                            int targetNo,
                                            [In] int[] targetCoords,
                                            double sizeX, double sizeY,
                                            [In] double[] targetOffsetsInCm,
                                            String videoName,
                                            [In, Out, MarshalAs(UnmanagedType.LPStruct)]
                                                    AnalysisResult ret);


        [DllImport(linuxSharedLibrary, EntryPoint = "isRigidPerspectiveTransformCS")]
        private static extern bool isRigidPerspectiveTransformCSLinux(int len,
                                                                    [In] double[] pointsSrcSpaceFlat,
                                                                    [In] double[] pointsDstSpaceFlat);

        [DllImport(linuxSharedLibrary, EntryPoint = "pointBaseChangeCS")]
        private static extern void pointBaseChangeCSLinux(double x, double y,
                                                        int len,
                                                        [In] double[] pointsSrcSpaceFlat,
                                                        [In] double[] pointsDstSpaceFlat,
                                                        [In, Out, MarshalAs(UnmanagedType.LPStruct)]
                                                        DumPoint2d ret);

        [DllImport(linuxSharedLibrary, EntryPoint = "srcPointToCmVectorFromTargetPointCS")]
        private static extern
            void srcPointToCmVectorFromTargetPointCSLinux(double src_space_point_x,
                                                        double src_space_point_y,
                                                        double cm_target_point_x,
                                                        double cm_target_point_y,
                                                        int len,
                                                        [In]
                                                        double[] pointsSrcSpaceFlat,
                                                        [In]
                                                        double[] pointsDstSpaceFlat,
                                                        [In, Out,
                                                        MarshalAs(UnmanagedType.LPStruct)]
                                                        DumPoint2d ret);
    }
}
