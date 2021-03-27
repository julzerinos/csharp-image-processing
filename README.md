# Image processing in C# Winforms

This project aims to recreat various image/bitmap processing methods and algorithms in a C# Winforms environment.  

## Image processing features

The following options are available for image processing.

### Image processor window

 * Displays the original and modified image side by side
 * You can directly copy from the modified image context menu
 * You can select the image view mode from "fit" and "view original size" 

<p align="center">
  <img src="https://raw.githubusercontent.com/julzerinos/csharp-image-processing/assets/1.png">
</p>

### Filters

 * You can apply a wide variety of functional as well as convolution filters
 * You can add as many layers as you like
 * Order of applying filters is taken into account
 * A caching system stores each bitmap for a given order of filter layers

<p align="center">
  <img src="https://raw.githubusercontent.com/julzerinos/csharp-image-processing/assets/2.png">
</p>

### Custom convolution kernel

 * Adds a custom kernel to the filters list
 * You can edit the kernel setup in a the Edit Kernel window
 * You can choose row and column count, kernel divisor (by default set automatically to sum of all cells), value offset and anchor point (wrt. applied cell)

<p align="center">
  <img src="https://raw.githubusercontent.com/julzerinos/csharp-image-processing/assets/3.png">
</p>

### Filter options window

* Changes the values used in certain filters
* Reapplies all current layers

<p align="center">
  <img src="https://raw.githubusercontent.com/julzerinos/csharp-image-processing/assets/4.png">
</p>

### YCbCr colorspace split

* Displays the current (modified) image [YCbCr colorspace](https://en.wikipedia.org/wiki/YCbCr) split

<p align="center">
  <img src="https://raw.githubusercontent.com/julzerinos/csharp-image-processing/assets/5.png">
</p>

## Bitmap creation features

The following options are available for bitmap creation (accessible via the Drawing window)

### Drawing window

 * A seperate window for all your bitmap creation needs
 * Allows saving and opening states (stored in serialized binary files)

<p align="center">
  <img src="https://raw.githubusercontent.com/julzerinos/csharp-image-processing/assets/6.png">
</p>

### Shapes

 * Various shapes can be created simply by clicking
 * These are vector object shapes, stored and treated as objects (ie. circle has an origin point and a radius)
 * All shapes can be moved, redrawn or removed in the shapes menu (color and thickness taken into account while redrawing)
 * Polygons may be clipped

<p align="center">
  <img src="https://raw.githubusercontent.com/julzerinos/csharp-image-processing/assets/7.png">
</p>

### Additional setup

 * Background color may be changed
 * Anti-aliasing may be turned on and off for shapes using midpoint lines
 * The clipping boundary may be displayed

<p align="center">
  <img src="https://raw.githubusercontent.com/julzerinos/csharp-image-processing/assets/8.png">
</p>

### Filling

 * Point-based flood fill is available (and stored as separate object)
 * Polygons may be filled with specific colors or tiled images

<p align="center">
  <img src="https://raw.githubusercontent.com/julzerinos/csharp-image-processing/assets/9.png">
</p>

## Final note

If for some darn reason good ol' Microsoft Paint ain't satisfying all your image dillydallying needs, head on down to Joe's Peppercot Farms n' try out that dandy release in the right-as-the-Lord-himself -hand menu. God bless 🤠
