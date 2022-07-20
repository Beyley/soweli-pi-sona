using ChaiFoxes.FMODAudio;
using FMOD;
using sowelipisona.Effects;

namespace sowelipisona.Fmod.Effects; 

public class FmodLowPassFilterAudioEffect : LowPassFilterAudioEffect {
	private DSP _dsp;
	private int _dspIndex;
	public FmodLowPassFilterAudioEffect(AudioStream stream) : base(stream) {}
	
	public override double FrequencyCutoff {
		get => this._frequencyCutoff;
		set {
			if (!this.Applied)
				throw new Exception("You must apply the effect before changing its parameters.");
			
			this._frequencyCutoff = value;
			
			this._dsp.setParameterFloat((int)DSP_MULTIBAND_EQ.A_FREQUENCY, (float)this.FrequencyCutoff);
		}
	}

	private FmodAudioStream AsFmodAudioStream() => (FmodAudioStream)this.Stream;

	protected override void InternalApply() {
		if (this.Applied)
			throw new Exception("You cannot apply an effect twice!");

		FmodAudioStream stream = this.AsFmodAudioStream();

		CoreSystem.Native.createDSPByType(DSP_TYPE.MULTIBAND_EQ, out this._dsp);
		
		this._dsp.setParameterInt((int)DSP_MULTIBAND_EQ.A_FILTER, (int)DSP_MULTIBAND_EQ_FILTER_TYPE.LOWPASS_24DB);
		this._dsp.setParameterFloat((int)DSP_MULTIBAND_EQ.A_FREQUENCY, (float)this.FrequencyCutoff);
		this._dsp.setParameterFloat((int)DSP_MULTIBAND_EQ.A_Q, 1f);

		this._dspIndex = stream.DspIndex++;
		stream.Channel.addDSP(this._dspIndex, this._dsp);
	}
	protected override void InternalRemove() {
		if (!this.Applied)
			throw new Exception("You must apply the effect first!");
		
		FmodAudioStream stream = this.AsFmodAudioStream();
		
		stream.Channel.removeDSP(this._dsp);
	}
}
