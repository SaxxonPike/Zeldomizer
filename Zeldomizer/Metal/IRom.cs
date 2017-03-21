﻿namespace Zeldomizer.Metal
{
    public interface IRom : Disaster.Assembly.Interfaces.IRom
    {
        void Copy(int source, int destination, int length);
        byte[] Read(int offset, int length);
        void Write(byte[] source, int destination);
        byte[] Export();
        byte[] ExportRaw();
    }
}