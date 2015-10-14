using UnityEngine;
using System.Collections;

public class Math3d {

    //Get the intersection between a line and a plane. 
    //If the line and plane are not parallel, the function outputs true, otherwise false.
    public static bool LinePlaneIntersection(out Vector3 intersection, Vector3 linePoint, Vector3 lineVec, Vector3 planeNormal, Vector3 planePoint)
    {

        float length;
        float dotNumerator;
        float dotDenominator;
        Vector3 vector;
        intersection = Vector3.zero;

        //calculate the distance between the linePoint and the line-plane intersection point
        dotNumerator = Vector3.Dot((planePoint - linePoint), planeNormal);
        dotDenominator = Vector3.Dot(lineVec, planeNormal);

        //line and plane are not parallel
        if (dotDenominator != 0.0f)
        {
            length = dotNumerator / dotDenominator;

            //create a vector from the linePoint to the intersection point
            vector = lineVec * length;

            //get the coordinates of the line-plane intersection point
            intersection = linePoint + vector;

            return true;
        }

        //output not valid
        else
        {
            return false;
        }
    }
}
