using ManagedBass;
using ManagedBass.Fx;
using sowelipisona.Effects;

namespace sowelipisona.ManagedBass.Effects;

public sealed class ManagedBassHighPassFilterAudioEffect : HighPassFilterAudioEffect {
    private readonly BQFParameters _parameters;
    
    public ManagedBassHighPassFilterAudioEffect(AudioStream stream) : base(stream) {
        this._parameters = new BQFParameters {
            fBandwidth = 0f,
            fCenter    = (float)this.FrequencyCutoff,
            lFilter    = BQFType.HighPass,
            fQ         = 1f
        };
    }

    private int _handle;

    public override double FrequencyCutoff {
        get => this._frequencyCutoff; 
        set {
            if (!this.Applied)
                throw new InvalidOperationException("You must apply an effect before changing its frequency cutoff!");

            this._parameters.fCenter = (float)value;
            this._frequencyCutoff    = value;
            
            this.SetParams();
        }
    }

    protected override void InternalApply() {
        if (this.Applied)
            throw new InvalidOperationException("You cannot apply an effect twice!");

        this._handle = Bass.ChannelSetFX(this.Stream.Handle, EffectType.BQF, 1);

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