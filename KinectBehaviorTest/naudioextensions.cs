using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

//from http://msdn.microsoft.com/en-us/magazine/ee309883.aspx
class SineWaveOscillator : WaveProvider16
{
    double phaseAngle;

    public SineWaveOscillator(int sampleRate) :
        base(sampleRate, 1)
    {
    }

    public double Frequency { set; get; }
    public short Amplitude { set; get; }

    public override int Read(short[] buffer, int offset,
      int sampleCount)
    {

        for (int index = 0; index < sampleCount; index++)
        {
            buffer[offset + index] =
              (short)(Amplitude * Math.Sin(phaseAngle));
            phaseAngle +=
              2 * Math.PI * Frequency / WaveFormat.SampleRate;

            if (phaseAngle > 2 * Math.PI)
                phaseAngle -= 2 * Math.PI;
        }
        return sampleCount;
    }
}