using FmodAudio.Base;
using FmodAudio.DigitalSignalProcessing;
using FmodAudio.DigitalSignalProcessing.Effects;
using sowelipisona.Effects;

namespace sowelipisona.Fmod.Effects; 

public class FmodReverbAudioEffect : ReverbAudioEffect {
	private Dsp _dsp;
	private int _dspIndex;
	public FmodReverbAudioEffect(AudioStream stream) : base(stream) {}

	private FmodAudioStream AsFmodAudioStream() => (FmodAudioStream)this.Stream;

	public override double ReverbDropoff {
		get => this._reverbDropoff;
		set {
			if (!this.Applied)
				throw new Exception("You must apply the effect before changing its parameters.");
			
			this.CheckDropoff(value);

			this._reverbDropoff = value;
			this._dsp.SetParameterFloat((int)DspSfxReverb.WetLevel, (float)this.ReverbDropoff);
		}
	}
	public override double ReverbTime {
		get => this._reverbTime;
		set {
			if (!this.Applied)
				throw new Exception("You must apply the effect before changing its parameters.");

			this.CheckTime(value);

			this._reverbTime = value;
			this._dsp.SetParameterFloat((int)DspSfxReverb.EarlyDelay, (float)this.ReverbTime);
		}
	}
	
	protected override void InternalApply() {
		if (this.Applied)
			throw new Exception("You cannot apply an effect twice!");

		FmodAudioStream stream = this.AsFmodAudioStream();

		this._dsp = stream.System.CreateDSPByType(DSPType.SFXReverb);
		
		this._dsp.SetParameterFloat((int)DspSfxReverb.EarlyDelay, (float)this.ReverbTime);
		this._dsp.SetParameterFloat((int)DspSfxReverb.WetLevel, (float)this.ReverbDropoff);
		// this._dsp.SetParameterFloat((int)DspSfxReverb.LateDelay, 0);
		// this._dsp.SetParameterFloat((int)DspMultibandEq.A_Frequency, (float)this.FrequencyCutoff);
		// this._dsp.SetParameterFloat((int)DspMultibandEq.A_Q, 1f);

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
