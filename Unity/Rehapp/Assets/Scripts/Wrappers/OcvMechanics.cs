using System.Runtime.InteropServices;
using System;

public class OcvMechanics {

#if UNITY_STANDALONE_WIN || UNITY_EDITOR

    [DllImport("RehappDll", EntryPoint = "?Test@Functions@RehappDll@@SAMXZ")]
    private static extern float Test();

    [DllImport("RehappDll", EntryPoint = "?matchTemplateVector@Functions@RehappDll@@SAMPEAMH0H@Z")]
    unsafe private static extern float matchTemplateVector(float* data, int dataSize, float* templ, int templSize);

    [DllImport("RehappDll", EntryPoint = "?BodyTrack@Functions@RehappDll@@SAXPEAEHH0PEAH1@Z")]
    private static extern void BodyTrack(byte[] raw, int width, int height, byte[] rawB, IntPtr hullPoints, out int hullLen);
    
    [DllImport("RehappDll", EntryPoint = "?TargetSelector@Functions@RehappDll@@SAXPEAEHHPEAH1@Z")]
    private static extern void TargetSelector(byte[] img, int imgWidth, int imgHeight, out int targetCenterX, out int targetCenterY);

    [DllImport("RehappDll", EntryPoint = "?matchTemplateImg@Functions@RehappDll@@SAXPEAEHH0HHPEANPEAH2@Z")]
    private static extern void matchTemplateImg(byte[] img, int imgWidth, int imgHeight, byte[] target, int targetWidth, int targetHeight, out double matchVal, out int matchCenterX, out int matchCenterY);

#else

    [DllImport("libRehapp")]
    private static extern float Test();

    [DllImport("libRehapp")]
    unsafe private static extern float matchTemplateVector(float* data, int dataSize, float* templ, int templSize);

    [DllImport("libRehapp")]
    private static extern void BodyTrack(byte[] raw, int width, int height, byte[] rawB, IntPtr hullPoints, out int hullLen);
    
    [DllImport("libRehapp")]
    private static extern void TargetSelector(byte[] img, int imgWidth, int imgHeight, out int targetCenterX, out int targetCenterY);

    [DllImport("libRehapp")]
    private static extern void matchTemplateImg(byte[] img, int imgWidth, int imgHeight, byte[] target, int targetWidth, int targetHeight, out double matchVal, out int matchCenterX, out int matchCenterY);    


#endif

    public static float DllTest()
    {
        return Test();
    }

    public static unsafe float MatchTemplate(float* data, int dataSize, float* templ, int templSize)
    {
        return matchTemplateVector(data, dataSize, templ, templSize);
    }

    public static unsafe void GetBodyTrack(byte[] raw, int width, int height, byte[] rawB, int[] hullPoints, out int hullLen)
    {
        fixed (int* p = hullPoints)
        {
            BodyTrack(raw, width, height, rawB, (IntPtr)p, out hullLen);
        }
    }

    public static unsafe void GetTarget(byte[] img, int imgWidth, int imgHeight, out int targetCenterX, out int targetCenterY)
    {
        TargetSelector(img, imgWidth, imgHeight, out targetCenterX, out targetCenterY);
    }

    public static unsafe void MatchTemplateImg(byte[] img, int imgWidth, int imgHeight, byte[] target, int targetWidth, int targetHeight, out double matchVal, out int matchCenterX, out int matchCenterY)
    {
        matchTemplateImg(img, imgWidth, imgHeight, target, targetWidth, targetHeight, out matchVal, out matchCenterX, out matchCenterY);
    }
}
