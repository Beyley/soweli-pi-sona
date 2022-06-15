using ManagedBass;
using ManagedBass.DirectX8;
using ManagedBass.Fx;
using sowelipisona.Effects;

namespace sowelipisona.ManagedBass.Effects;

public sealed class ManagedBassReverbAudioEffect : ReverbAudioEffect {
    private readonly DXReverbParameters _parameters;
    
    public ManagedBassReverbAudioEffect(AudioStream stream) : base(stream) {
        this._parameters = new DXReverbParameters {
            fHighFreqRTRatio = 0.001f,
            fReverbMix       = (float)this.ReverbDropoff,
            fReverbTime      = (float)this.ReverbTime
        };
    }

    private int _handle;

    public override double ReverbDropoff {
        get => this._reverbDropoff;
        set {
            if (!this.Applied)
                throw new Exception("You must apply the effect before changing its parameters.");
            
            this.CheckDropoff(value);
            
            this._reverbDropoff        = value;
            this._parameters.fReverbMix = (float)value;

            this.SetParams();
        }
    }
    public override double ReverbTime {
        get => this._reverbTime;
        set {
            if (!this.Applied)
                throw new Exception("You must apply the effect before changing its parameters.");
            
            this.CheckTime(value);
            
            this._reverbTime            = value;
            this._parameters.fReverbTime = (float)value;
            
            this.SetParams();
        }
    }
    protected override void InternalApply() {
        if (this.Applied)
            throw new InvalidOperationException("You cannot apply an effect twice!");

        this._handle = Bass.ChannelSetFX(this.Stream.Handle, EffectType.DXReverb, 1);

        if (this._handle == 0)
            throw new Exception($"Unable to set the channel FX! err:{Bass.LastError}");
        
        this.SetParams();
    }

    private void SetParams() {
        bool succeeded = Bass.FXSetParameters(this._handle, this._parameters);

        if (!succeeded)
            throw new Exception($"Unable to set effect parameters! err:{Bass.LastError}");
    }

    protected override void InternalRemove() {
        if (!this.Applied)
            throw new InvalidOperationException("You cannot remove an effect if it is not applied");

        bool success = Bass.ChannelRemoveFX(this.Stream.Handle, this._handle);

        if (!success)
            throw new Exception($"Unable to remove effect! err:{Bass.LastError}");
    }
}