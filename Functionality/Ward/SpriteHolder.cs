using UnityEngine;

namespace ExtendedStay.Functionality.Ward
{
    public abstract class SpriteHolder
    {
        public SpriteHolder(Character character, string customCharacter)
        {
            gameObject = new($"Mod_{MyPluginInfo.PLUGIN_GUID}_LevelCharacter");
            gameObject.SetActive(false);

            Transform transform = gameObject.transform;

            transform.SetParent(scnGame.instance.rooms[0].transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            GameObject sprite = GameObject.Instantiate(RDConstants.data.customSprite, transform);
            this.sprite = sprite.GetComponent<CustomSprite>();

            CustomAnimation animation = sprite.GetComponent<CustomAnimation>();
            if (character == Character.Custom)
            {
                if (scnGame.instance.currentLevel.customCharacterData.TryGetValue(customCharacter, out CustomAnimationData data))
                {
                    animation.data = data;
                }
                else
                {
                    animation.data = scrChar.baseCharacterAnimations[Character.Samurai];
                }
            }
            else
            {
                animation.data = scrChar.baseCharacterAnimations[character];
            }

            gameObject.SetActive(true);
            this.transform = transform;
            this.animation = animation;

            PlayExpression("neutral");
        }

        protected void PlayExpression(string expression, float speed = 1f)
        {
            if (animation.data.clips.TryGetValue(expression, out CustomAnimationClip clip))
            {
                animation.PlayFromClip(clip, speed, 0f, 0f);
            }
        }

        protected void CleanUp()
        {
            GameObject.Destroy(gameObject);
        }

        protected readonly GameObject gameObject = null;
        protected readonly Transform transform = null;
        protected readonly CustomSprite sprite = null;
        protected readonly CustomAnimation animation = null;
    }
}
