using System;

namespace aoc.Internal
{
    sealed class ParticleHelperStrategy : Helper2Strategy<ParticleHelperStrategy>
    {
        private ParticleHelperStrategy()
            : base("p", "v", "a")
        {
        }

        public override int MinCount => 2;
        public override int MaxCount => 3;

        public override char DefaultSeparator => '@';
    }

    sealed class ParticleHelper<TParticle, TVector> : Helper2<TParticle, TVector, ParticleHelperStrategy>
        where TParticle : struct, IParticle<TParticle, TVector>
        where TVector : struct, IFormattable
    {
        public ParticleHelper(Func<TVector[], TParticle> fromArray, TryParseValue2<TVector> tryParse)
            : base(fromArray, tryParse)
        {
        }

        protected override ParticleHelperStrategy Strategy =>
            ParticleHelperStrategy.Instance;
    }
}
