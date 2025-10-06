using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Corpus_Callosum.AnimHandler
{
    public class CorpusAnim
    {
        public string animName;
        public string filePath;
        public int fps;
        public tk2dSpriteAnimationClip.WrapMode wrapMode;
        public int numberOfFrames;
        public int spriteXBound;
        public int spriteYBound;
    }
}
