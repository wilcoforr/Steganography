using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Steganography
{
    public partial class Steganography : Form
    {

        //change the image file path location depending on whether building as debug or release
        //right now this is only for filesaving...
        //if the image wants to be changed you will have to use visual studio and in the design view change the image.
        //then exit visual studio, then restart/rebuild the binary (.exe)
#if DEBUG
        string imageFilePath = Environment.CurrentDirectory + @"..\..\..\test_image.jpg";
#else
        string imageFilePath = Environment.CurrentDirectory + @"\test_image.jpg";
#endif



        public Steganography()
        {
            InitializeComponent();
        }


        /// <summary>
        /// no bit datatype in C# so using int - 0 and 1
        /// Turn the array of chars into a list of bits (stored as ints)
        /// </summary>
        /// <param name="messageToEncode">a string - but used like an array of characters (as bytes) to encode the message</param>
        /// <returns>a list of 0's and 1's for what bits to encode</returns>
        private List<int> CreateListOfBits(string messageToEncode)
        {
            List<int> bitsToEncode = new List<int>();

            byte[] asciiChars = messageToEncode.Select(c => (byte)c).ToArray();

            //build the bitsToEncode list
            foreach (byte character in asciiChars)
            {
                for (int i = 7; i > -1; i--)
                {
                    //shift character then AND the character to see if it is 0 - then add a 0 to the list
                    int andedCharacter = (character >> i) & 0x01;

                    if (andedCharacter == 0)
                    {
                        bitsToEncode.Add(0);
                    }
                    //else add a 1
                    else
                    {
                        bitsToEncode.Add(1);
                    }
                }
            }

            return bitsToEncode;
        }


        /// <summary>
        /// Hides the bit in the byte[0] place - ie 0000 000i 
        /// </summary>
        /// <param name="colorValue">the color byte value - RGB - for this program it's red only</param>
        /// <param name="bitToHide">either a 0 or 1</param>
        /// <returns>a new byte with that has the hidden bit inside</returns>
        private byte AddBitToHideInByte(byte colorValue, int bitToHide)
        {
            if (bitToHide == 0)
            {
                //For example, if the red (color.R) is 53 and we want to encode 0 to give us the value 52
                //11111110 = 0xFE = 254
                return colorValue = Convert.ToByte(colorValue & 254);
            }
            else
            {
                //If the red is 54 (color.R) and we want to encode 1, to give us the value 55
                //00000001 = 0x01 = 1
                return colorValue = Convert.ToByte(colorValue | 1);
            }
        }


        /// <summary>
        /// on user click - checks to see if the length of the message is greater than the number of pixels / 8
        /// if it is, then an error message is shown because the whole message can't be encoded.
        /// </summary>
        private void btn_encode_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Encoding message. Press OK or Enter to continue.", "Please read");

            //divided by 8 because 1 pixel hold one bit part each
            int charsImageCanHold = (pb_image.Width * pb_image.Height) / 8;

            if (charsImageCanHold < txtbx_message.Text.Length)
            {
                MessageBox.Show("Too many characters to encode... " + txtbx_message.Text.Length + "/" + charsImageCanHold + " max.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //get the list of bits to encode

            List<int> bitsToEncode = CreateListOfBits(txtbx_message.Text);

            //reset textbox to be empty because message is now hidden
            txtbx_message.Text = "";

            btn_decode.Enabled = false;
            btn_encode.Enabled = false;
            EncodeBitsIntoImage(bitsToEncode);
            btn_decode.Enabled = true;
            btn_encode.Enabled = true;
        }


        /// <summary>
        /// One pixel is either increased or decreased for the bit to hide the message in.
        /// 8 pixels in a row make a byte representing the ASCII char hidden/encoded
        /// </summary>
        /// <param name="bitsToEncode">list of bits (0 or 1s) to encode into each pixel</param>
        private void EncodeBitsIntoImage(List<int> bitsToEncode)
        {

            //cant dispose properly with using resource/memory leak? maybe because of return statement.
            //I guess Dispose is called on the way out - http://stackoverflow.com/questions/2369887/are-there-any-side-effects-of-returning-from-inside-a-using-statement
            using (var encodedImage = new Bitmap(pb_image.Image))
            {
                int i = 0;

                for (int x = 0; x < encodedImage.Width; x++)
                {
                    for (int y = 0; y < encodedImage.Height; y++)
                    {
                        //color is a struct with a byte representing RGB for each color
                        Color color = encodedImage.GetPixel(x, y);


                        //make the Red byte component of the RGB color struct with the hidden bit to hide
                        if (bitsToEncode.Any())
                        {
                            byte red = AddBitToHideInByte(color.R, bitsToEncode.First());

                            //remove the bit from the list - should've probably used a queue but whatever
                            bitsToEncode.Remove(bitsToEncode.First());

                            Color colorWithHiddenBit = Color.FromArgb(red, color.G, color.B);

                            encodedImage.SetPixel(x, y, colorWithHiddenBit);

                        }
                        //list is empty
                        else
                        {
                            //encode 00000000 to mark the end of the text
                            if (i < 8)
                            {
                                byte red = AddBitToHideInByte(color.R, 0);

                                Color colorWithEndOfText = Color.FromArgb(red, color.G, color.B);

                                encodedImage.SetPixel(x, y, colorWithEndOfText);
                                i++;
                            }
                            else
                            {
                                try
                                {
                                    var converter = new ImageConverter();

                                    byte[] encodedImageByteArray = (byte[])converter.ConvertTo(encodedImage, typeof(byte[]));
                                    System.IO.File.WriteAllBytes(imageFilePath, encodedImageByteArray);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);

                                }

                                //set image back to display - still leaks memory though. Not sure how to fix.
                                //maybe the return before the using statement finishes/goes out of scope? leaks the memory
                                pb_image.Image = new Bitmap(encodedImage);

                                return;
                            }
                        }
                    } //height
                } //width
            } //using

        }


        /// <summary>
        /// This gets 8 pixels in a row and tries to make a byte (character) out of the hidden bits
        /// </summary>
        private void btn_decode_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Decoding text from image. Press OK or Enter to show decoded text.", "Please read");
            //btn_encode.Enabled = true;
            //btn_decode.Enabled = false;
            //txtbx_message.Show();

            txtbx_message.Text = "";

            DecodeBitsFromImage();
        }


        /// <summary>
        /// decoded message to put back into the textbox
        /// </summary>
        private void DecodeBitsFromImage()
        {
            //garbage/memory management
            using (var decodedImage = new Bitmap(imageFilePath))
            {
                //keeps track of the part of the byte to shift
                int i = 7;

                //decoded ASCII character that will be converted back to a char and appended to the message
                int hiddenNumber = 0;

                for (int x = 0; x < decodedImage.Width; x++)
                {
                    for (int y = 0; y < decodedImage.Height; y++)
                    {
                        Color color = decodedImage.GetPixel(x, y);

                        //get the encoded bit from the Red byte of the color (color.R) - 0 if even, 1 if odd
                        int bit = ((color.R % 2) == 0) ? 0 : 1;

                        byte shiftedByte = Convert.ToByte(bit * Math.Pow(2, i));
                        //create shifted byte
                        //byte shiftedByte = Convert.ToByte(bit << i);

                        hiddenNumber = Convert.ToByte(hiddenNumber + shiftedByte);

                        i--;

                        //end of "byte index" for the byte/ASCII char array -- i = -1
                        if (i < 0)
                        {
                            char charToAddToMessage = Convert.ToChar(hiddenNumber);
                            //message has been read - now on an ASCII char of 0000 0000 (ie decodedAsciiCharacter = 0000 0000)
                            //this is the "NULL \0" character for this to break out of the loops with a return
                            if (charToAddToMessage == 0)
                            {
                                return;
                            }

                            //only add a character to the message if it is a letter/digit/whitespace
                            if (Char.IsLetterOrDigit(charToAddToMessage) || Char.IsWhiteSpace(charToAddToMessage))
                            {
                                //append character to textbox
                                txtbx_message.Text += charToAddToMessage;
                            }

                            //reset i and decodedAsciiCharacter to decode the next byte 
                            i = 7;

                            hiddenNumber = 0;
                        }

                    }//height

                }//width

            }//using

        }


    }
}
