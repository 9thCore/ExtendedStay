using ExtendedStay.Util;
using RhythmWeightlifter;
using System;
using UnityEngine;

namespace ExtendedStay.Functionality.Ward
{
    using LevelStorage = Functionality.Level.Storage;

    public class Level : SpriteHolder, ISelectable, IObject
    {
        public LevelStorage.Data Data
        {
            get
            {
                return data;
            }
        }

        public Vector2 Position => transform.position;
        public Vector2 DescriptionPosition => new(Position.x + data.DescriptionOffset.x - scrVfxControl.instance.RDWidth / 2f, data.DescriptionOffset.y);
        public Rank CurrentRank => RankUtil.GetHighestRank(data.Hash);

        public Level(LevelStorage.Data data) : base(data.Character, data.CustomCharacter)
        {
            this.data = data;
            transform.position = data.Position.AsPercent();

            animation.onAnimationCompleted = (Action<CustomAnimation, CustomAnimationClip>)
                Delegate.Combine(
                    animation.onAnimationCompleted,
                    new Action<CustomAnimation, CustomAnimationClip>(OnAnimationEnd));

            UpdateExpression(skipUnplayed: false);
        }

        public void OnHover()
        {
            UpdateExpression(skipUnplayed: true);
        }

        public void OnInteract()
        {

        }

        public void Destroy()
        {
            CleanUp();
        }

        private void OnAnimationEnd(CustomAnimation animation, CustomAnimationClip clip)
        {
            if (clip.name == "neutral" || animation.isLoopOnBeat)
            {
                return;
            }

            PlayExpression("neutral");
        }

        private void UpdateExpression(bool skipUnplayed)
        {
            if (!skipUnplayed && CurrentRank.Unplayed())
            {
                PlayExpression(data.UnplayedExpression);
            }
            else if (CurrentRank.perfected)
            {
                PlayExpression(data.PerfectExpression);
            }
            else if (!CurrentRank.passed)
            {
                PlayExpression(data.FailExpression);
            }
        }
        
        private readonly LevelStorage.Data data;
    }
}
