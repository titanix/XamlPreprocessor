You need to modify the .projitems in the following way for each XAML file (sample.xaml) you want to preprocess:
- make a copy of the file in the same folder and rename it sample.pre.xaml
- add None element which sample.pre.xaml in the ItemGroup
- change or add the DependentUpon element of sample.xaml and sample.xaml.cs to reference sample.