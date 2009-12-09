using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;
using System.IO;
using System.Collections.Generic;

namespace Arbaureal.Arcle.Engine
{
    public class SoundBufferArray
    {
        int nCurrentSoundIndex;
        List<MediaElement> listSounds;

        public SoundBufferArray(Uri soundUri, int nBufferSize, Panel LayoutRoot)
        {
            nCurrentSoundIndex = 0;
            listSounds = new List<MediaElement>(nBufferSize);
            
            StreamResourceInfo sri = Application.GetResourceStream(soundUri);

            if (sri != null && sri.Stream != null)
            {
                Byte[] arr = new Byte[sri.Stream.Length];
                sri.Stream.Read(arr, 0, (int)sri.Stream.Length);

                for (int index = 0; index < nBufferSize; ++index)
                {
                    MemoryStream memstream = new MemoryStream(arr);
                    MediaElement media = new MediaElement();
                    media.SetSource(memstream);
                    media.AutoPlay = false;
                    LayoutRoot.Children.Add(media);
                    listSounds.Add(media);
                }
            }
        }

        public void PlaySoundFromBuffer()
        {
            if (nCurrentSoundIndex >= listSounds.Count)
            {
                nCurrentSoundIndex = 0;
            }

            listSounds[nCurrentSoundIndex].Stop();
            listSounds[nCurrentSoundIndex].Play();

            ++nCurrentSoundIndex;
        }
    }
}
