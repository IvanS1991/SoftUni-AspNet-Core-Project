class CreatePropertyForm
{
    constructor() {
        this.$selects = $('select.form-control');
        this.$imageInput = $('input.image-upload');
        this.$imagePreview = $('div.image-preview');
        this.districtId = null;
        this.propertyTypeId = null;
    }

    listenForPreview() {
        this.$imageInput.on('input',
            (evt) => this.updatePreview(evt.target));
    }

    listenForDistrictAndPropertyType() {
        this.$selects.on('change', (e) => {
            switch (e.target.id) {
                case 'DistrictId':
                    this.districtId = e.target.value;
                    break;
                case 'TypeId':
                    this.propertyTypeId = e.target.value;
                    break;
            }

            if (this.districtId && this.propertyTypeId) {
                this.populateAggregatesSection();
            }
        });
    }

    populateAggregatesSection() {
        const $aggregateData = $('#aggregateData');

        $aggregateData.empty();

        $.get(`/api/property-aggregates?districtid=${this.districtId}&propertytypeid=${this.propertyTypeId}`,
            (response) => {
                $aggregateData.html(`Средна цена за ${response.propertyType.name} в ${response.district.name} - ${response.averagePrice}`);
            });

        $('form.create-property-form > *').removeClass('hidden');
    }

    updatePreview(input) {
        this.$imagePreview.empty();

        Array.from(input.files)
            .forEach((x, idx) => this
                .addImageToPreview(this.$imagePreview, x, idx));
    }

    createRemoveImageButton(index) {
        const $button = $('<button></button>')
            .addClass(['btn', 'btn-danger'])
            .attr('type', 'button');

        $button.html('x');
        $button.on('click', () => {
            const dt = new DataTransfer();
            const input = this.$imageInput[0];
            const { files } = input;

            for (let i = 0; i < files.length; i++) {
                const file = files[i];

                if (index !== i) {
                    dt.items.add(file);
                }

                input.files = dt.files;
            }

            this.updatePreview(input);
        });

        return $button;
    }

    createImageElement(image) {
        const $image = $('<img/>');

        $image.attr('src', URL.createObjectURL(image));
        $image.on('load', () => {
            URL.revokeObjectURL($image.attr('src'));
        });

        return $image;
    }

    addImageToPreview($imagePreview, image, index) {
        const $container = $('<div></div>').addClass('image-preview-container');

        $container.append(this.createImageElement(image));
        $container.append(this.createRemoveImageButton(index));

        this.$imagePreview.append($container);
    };
}