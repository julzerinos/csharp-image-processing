# Winforms Image Processing

An application created in Winforms under C# for processing images. Allows applying various function and convolution filters among other functionalities.

## Version specifications

### Version v1.0

The initial version is specified by the following requirements:

* Loading of a selected image file and displaying it in the application window
* Applying selected filters to the loaded image and displaying result beside
or in place of original image
* Combining multiple filters on top of each other
* Saving result image to a file
* Returning filtered image back to its original state without reloading the
file
* Implementation of following function filters - with fixed parameters easily
modifiable from the source code:
  * inversion,
  * brightness correction,
  * contrast enhancement,
  * gamma correction.
* Implementation of following convolution filters
  * box blur,
  * Gaussian blur
  * sharpen,
  * edge detection
  * emboss
  
  Additionally, functionalities of convolution filters are extended by:
  
 * Separate area displaying rectangular grid with editable convolution filter
kernel coeﬀicients
* Independent selection of numbers of kernel columns and rows. Values can
be limited to an odd numbers from range [1, 9]
* Editable field with filter divisor
* Option to automatically compute divisor (sum of coefficients or 1 if the
sum is 0)
* Editable field with filter offset value
* Selection of kernel anchor point, i.e. element of a grid which overlaps with
currently processed pixel
* Loading and editing existing filters, including predefined convolution filters specified in the common part
* Saving created or modified filters in an application and applying them to
the image
