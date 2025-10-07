using Corpus_Callosum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Corpus_Callosum.AnimHandler
{
    public static class AnimManager
    {
        private static bool init = false;
        public const float SPRITESCALE = 3.5f;
        public static GameObject WeaverCrestAnimator;
        public static void InitAnimations()
        {
            if (init) return;
            init = true;

            #region WeaverCrest animations

            WeaverCrestAnimator = CreateAnimationObject("WeaverCrest");

            //WeaverCrest animations (for WeaverCrestDante)

            //SlashEffect
            LoadAnimationTo(WeaverCrestAnimator, "CORPUS.Assets.WeaverCrest.SlashEffects.Base.spritesheet.png", "SlashEffect", 30, tk2dSpriteAnimationClip.WrapMode.Once, 4, 217, 190);
            SetFrameToTrigger(WeaverCrestAnimator, "SlashEffect", 1); // To activate the damage frames within NailSlash
            SetFrameToTrigger(WeaverCrestAnimator, "SlashEffect", 3);

            //SlashAltEffect
            LoadAnimationTo(WeaverCrestAnimator, "CORPUS.Assets.WeaverCrest.SlashEffects.Alt.spritesheet.png", "SlashAltEffect", 30, tk2dSpriteAnimationClip.WrapMode.Once, 4, 266, 237);
            SetFrameToTrigger(WeaverCrestAnimator, "SlashAltEffect", 1); // To activate the damage frames within NailSlash
            SetFrameToTrigger(WeaverCrestAnimator, "SlashAltEffect", 3);

            //SlashUpEffect
            LoadAnimationTo(WeaverCrestAnimator, "CORPUS.Assets.WeaverCrest.SlashEffects.Up.spritesheet.png", "SlashUpEffect", 30, tk2dSpriteAnimationClip.WrapMode.Once, 4, 215, 241);
            SetFrameToTrigger(WeaverCrestAnimator, "SlashUpEffect", 1); // To activate the damage frames within NailSlash
            SetFrameToTrigger(WeaverCrestAnimator, "SlashUpEffect", 3);

            //DownslashEffect
            LoadAnimationTo(WeaverCrestAnimator, "CORPUS.Assets.WeaverCrest.SlashEffects.Downslash.spritesheet.png", "DownslashEffect", 20, tk2dSpriteAnimationClip.WrapMode.Once, 4, 1, 1);
            SetFrameToTrigger(WeaverCrestAnimator, "DownslashEffect", 0); // To activate the damage frames within NailSlash
            SetFrameToTrigger(WeaverCrestAnimator, "DownslashEffect", 3);

            //Downslash
            LoadAnimationTo(WeaverCrestAnimator, "CORPUS.Assets.WeaverCrest.Slashes.DownslashAntic.spritesheet.png", "Downslash", 16, tk2dSpriteAnimationClip.WrapMode.Once, 2, 270, 250);

            //Downslash Antic
            LoadAnimationTo(WeaverCrestAnimator, "CORPUS.Assets.WeaverCrest.Slashes.DownslashAntic.spritesheet.png", "Downslash Antic", 18, tk2dSpriteAnimationClip.WrapMode.Once, 3, 270, 250);

            //Dashstab Antic
            //LoadAnimationTo(WeaverCrestAnimator, "CORPUS.Assets.WeaverCrest.Downslash.spritesheet.png", "Downslash", 16, tk2dSpriteAnimationClip.WrapMode.Once, 2, 270, 250);

            //Dashstab
            LoadAnimationTo(WeaverCrestAnimator, "CORPUS.Assets.WeaverCrest.Slashes.Dashstab.spritesheet.png", "Dashstab", 18, tk2dSpriteAnimationClip.WrapMode.Once, 3, 313, 192);


            #endregion
        }

        private static void SetFrameToTrigger(GameObject animatorObject, string animationName, int frame)
        {
            animatorObject.GetComponent<tk2dSpriteAnimator>().Library.GetClipByName(animationName).frames[frame].triggerEvent = true;
        }

        private static GameObject CreateAnimationObject(string name)
        {
            GameObject obj = new GameObject();
            obj.name = name;
            obj.AddComponent<tk2dSprite>();
            tk2dSpriteAnimation animation = obj.AddComponent<tk2dSpriteAnimation>();
            tk2dSpriteAnimator animator = obj.AddComponent<tk2dSpriteAnimator>();
            animator.Library = animation;
            animation.clips = new tk2dSpriteAnimationClip[0];
            UnityEngine.GameObject.DontDestroyOnLoad(obj);
            UnityEngine.GameObject.DontDestroyOnLoad(animator);
            return obj;
        }

        private static void LoadAnimationTo(GameObject animator, string path, string name, int fps, tk2dSpriteAnimationClip.WrapMode wrapmode, int length, int xbound, int ybound)
        {
            List<tk2dSpriteAnimationClip> list = animator.GetComponent<tk2dSpriteAnimator>().Library.clips.ToList<tk2dSpriteAnimationClip>();

            Texture2D texture1 = ResourceLoader.LoadTexture2D(path);


            string[] names = new string[length];
            Rect[] rects = new Rect[length];
            Vector2[] anchors = new Vector2[length];

            for (int i = 0; i < length; i++)
            {
                names[i] = name + (i + 1).ToString();
                rects[i] = new Rect(i * xbound, i * ybound, xbound, ybound);
                anchors[i] = new Vector2(xbound / 2, ybound / 2);
            }

            tk2dSpriteCollectionData spriteCollectiondata = Tk2dHelper.CreateTk2dSpriteCollection(texture1, names, rects, anchors, new GameObject());
            spriteCollectiondata.material = HeroController.instance.GetComponent<MeshRenderer>().material;

            tk2dSpriteAnimationFrame[] list1 = new tk2dSpriteAnimationFrame[length];

            for (int i = 0; i < length; i++)
            {
                tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
                frame.spriteCollection = spriteCollectiondata;
                frame.spriteId = i;


                list1[i] = frame;
            }

            tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip();
            clip.name = name;
            clip.fps = fps;
            clip.frames = list1;
            clip.wrapMode = wrapmode;


            clip.SetCollection(spriteCollectiondata);

            list.Add(clip);
            tk2dSpriteAnimation animation = animator.GetComponent<tk2dSpriteAnimator>().Library;
            animation.clips = list.ToArray();
            Helper.SetPrivateField<bool>(animation, "isValid", false); //to refresh the animation lookup
            animation.ValidateLookup();
        }
    }
}