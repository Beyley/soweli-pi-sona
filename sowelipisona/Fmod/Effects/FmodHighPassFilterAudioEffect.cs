using FmodAudio.Base;
using FmodAudio.DigitalSignalProcessing;
using FmodAudio.DigitalSignalProcessing.Effects;
using sowelipisona.Effects;

namespace sowelipisona.Fmod.Effects; 

public class FmodHighPassFilterAudioEffect : HighPassFilterAudioEffect {
	private Dsp _dsp;
	private int _dspIndex;
	public FmodHighPassFilterAudioEffect(AudioStream stream) : base(stream) {}
	
	public override double FrequencyCutoff {
		get => this._frequencyCutoff;
		set {
			if (!this.Applied)
				throw new Exception("You must apply the effect before changing its parameters.");
			
			this._frequencyCutoff = value;
			
			this._dsp.SetParameterFloat((int)DspMultibandEq.A_Frequency, (float)this.FrequencyCutoff);
		}
	}

	private FmodAudioStream AsFmodAudioStream() => (FmodAudioStream)this.Stream;

	protected override void InternalApply() {
		if (this.Applied)
			throw new Exception("You cannot apply an effect twice!");

		FmodAudioStream stream = this.AsFmodAudioStream();

		this._dsp = stream.System.CreateDSPByType(DSPType.MultiBand_EQ);
		
		this._dsp.SetParameterInt((int)DspMultibandEq.A_Filter, (int)DspMultibandEqFilterType.Highpass_24DB);
		this._dsp.SetParameterFloat((int)DspMultibandEq.A_Frequency, (float)this.FrequencyCutoff);
		this._dsp.SetParameterFloat((int)DspMultibandEq.A_Q, 1f);

		this._dspIndex = stream.DspIndex++;
		stream.Channel.AddDSP(this._dspIndex, this._dsp);
	}
	protected override void InternalRemove() {
		if (!this.Applied)
			throw new Exception("You must apply the effect first!");
		
		FmodAudioStream stream = this.AsFmodAudioStream();
		
		stream.Channel.RemoveDSP(this._dsp);
	}
}
