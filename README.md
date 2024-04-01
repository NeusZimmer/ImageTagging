This is an app I decided to create as the option that I found for managing Image Datasets and its tags were not suitable for me. It's design was done with the idea of working with as many images as possible and avoid the repetition of some tasks.

![MainScreen](https://github.com/NeusZimmer/ImageTagging/assets/94193584/4d083f1c-2c6c-4e28-85f9-0d6d5c1dc250)


Main idea for your first try:

Open a directory containing a group of images, define the number of images to be displayed on screen and their thumbnail size (recommened for the first time try, then you may skip this), when you are ok, you may want to reopen the directory.
![ConfigScreen](https://github.com/NeusZimmer/ImageTagging/assets/94193584/fd19d300-3b4f-4bfc-bcf9-bcd50f461d70)

Examine your images and look for the areas/objects you will need to categorize in general, define a template to be used (you could apply templates to group of images). Apply the template to the desired images. Think on the number of areas you will manage in parallel (number of categories).
![template](https://github.com/NeusZimmer/ImageTagging/assets/94193584/61c291c0-e316-4b62-ba94-7fd850f5cb2a)

Tags: you may import txt files ( one tag per line) in their left panel, ( i.e.: wildcards txt files while you define your own tag files). Also, if you need a tag that is not included you may add it to the current categorie.
Tag category: think on the template you applied to the images, you have to define a destination for each of the categories, this will add the selected tags to the selected images, but only if that objetct/area alreay exists in the image template, if not it will be not added. You may use the combo or name it ( case sensitive) if it does not appear in the list
![tag_load](https://github.com/NeusZimmer/ImageTagging/assets/94193584/c386e772-3bf0-42e0-bec3-4b9611aac48c)
![Destination](https://github.com/NeusZimmer/ImageTagging/assets/94193584/6281f73a-fd24-473b-930b-2c7ba8a025ec)

Select the tags you want to add to selected images and repeat as needed this step.

When done, click on "save data" this will save the session information into json files (one for each image), you may want also to export it to a txt file for use it to train a model on your images.





