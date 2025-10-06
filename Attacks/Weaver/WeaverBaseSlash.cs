using Corpus_Callosum;
using Corpus_Callosum.AnimHandler;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Corpus_Callosum.Attacks.Weaver
{
    public class WeaverBaseSlash
    {
        public GameObject SlashObject { get; set; }
        public string SlashPrefabName { get; set; }
        public string AnimationName { get; set; }
        public GameObject SlashAnimatorObject { get; set; }
        public Vector3 LocalPosition { get; set; }
        public Vector3 LocalScale { get; set; }

        public void SetupSlash()
        {
            // Load prefab by name
            GameObject prefab = ResourceLoader.bundle.LoadAsset<GameObject>(SlashPrefabName);
            if (prefab == null)
            {
                throw new ArgumentException("Prefab '" + SlashPrefabName + "' not found in Corpus Collosum's AssetBundle!");
            }

            SlashObject.transform.localPosition = LocalPosition;
            SlashObject.GetComponent<NailSlash>().scale = LocalScale;
            SlashObject.GetComponent<NailSlash>().animName = AnimationName;
            SlashObject.GetComponent<NailSlash>().Awake();

            SlashObject.GetComponent<tk2dSpriteAnimator>().Library = SlashAnimatorObject.GetComponent<tk2dSpriteAnimator>().Library;

            PolygonCollider2D collider = SlashObject.GetComponent<PolygonCollider2D>();
            collider.points = prefab.GetComponent<PolygonCollider2D>().points;

            //To account for the increased anim size
            Helper.ScalePolygonCollider(collider, (float)Math.Sqrt(AnimManager.SPRITESCALE));
        }
    }
}