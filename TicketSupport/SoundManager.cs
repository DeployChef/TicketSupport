using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace TicketSupport
{
    public static class SoundManager
    {
        public static void PlayNewMessage()
        {
            var fileStream = Properties.Resources.here_i_am;
            PlaySound(fileStream);
        }

        private static void PlaySound(Stream s)
        {
            var player = new SoundPlayer(s);
            player.Play();
        }
    }
}
