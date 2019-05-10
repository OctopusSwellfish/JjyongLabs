using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SerialComm
{
    public class GrblCommand
    {
        string typeOfMovement = "G0 ";
        string newline = "\n";
        

        public string TestCommand()
        {
            return "$$" + newline;
        }

        public string FKMoveArt1(int value)
        {
            return typeOfMovement + "A" + value + newline;
        }

        public string FKMoveArt2(int value)
        {
            return typeOfMovement + "B" + value + "C" + value + newline;
        }

        public string FKMoveArt3(int value)
        {
            return typeOfMovement + "D" + value + newline;
        }

        public string FKMoveArt4(int value)
        {
            return typeOfMovement + "X" + value + newline;
        }

        public string FKMoveArt5(int value)
        {
            return typeOfMovement + "Y" + value + newline;
        }

        public string FKMoveArt6(int value)
        {
            return typeOfMovement + "Z" + value + newline;
        }
        
        public string ZeroPositionCommand()
        {
            return  typeOfMovement + "A0 B0 C0 D0 X0 Y0 Z0" + newline;
        }
    }
}

