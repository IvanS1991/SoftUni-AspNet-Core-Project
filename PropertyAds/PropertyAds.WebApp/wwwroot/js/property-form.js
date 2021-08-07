class PropertyForm
{
    constructor() {
        this.$selects = $('select.form-control');
        this.$districtSelect = $('select.form-control#DistrictId');
        this.$propertyTypeSelect = $('select.form-control#TypeId');
        this.$priceInput = $('#Price');
        this.$imageInput = $('input.image-upload');
        this.$imagePreview = $('div.image-preview');

        this.tryInitializeForm();
    }

    get districtId() {
        return this.$districtSelect.val();
    }

    get propertyTypeId() {
        return this.$propertyTypeSelect.val();
    }

    listenForPreview() {
        this.$imageInput.on('input',
            (evt) => this.updatePreview(evt.target));
    }

    listenForDistrictAndPropertyType() {
        this.$selects.on('change', (e) => {
            this.tryInitializeForm();
        });
    }

    listenForPriceChange() {
        this.$priceInput.on('input', (e) => {
            this.determinePriceDiff(e.target.value);
        });
    }

    determinePriceDiff(priceAverage) {
        const $diffUp = $('.diff-indicator.diff-up');
        const $diffDown = $('.diff-indicator.diff-down');

        if (priceAverage < this.currentAverage) {
            $diffUp.addClass('hidden');
            $diffDown.removeClass('hidden');
        } else if (priceAverage > this.currentAverage) {
            $diffUp.removeClass('hidden');
            $diffDown.addClass('hidden');
        } else {
            $diffUp.addClass('hidden');
            $diffDown.addClass('hidden');
        }
    }

    tryInitializeForm() {
        if (this.districtId && this.propertyTypeId) {
            this.populateAggregatesSection();
        }
    }

    populateAggregatesSection() {
        const $aggregateData = $('#aggregateData');

        $.get(`/api/property-aggregates/aggregate?districtid=${this.districtId}&propertytypeid=${this.propertyTypeId}`,
            (response) => {
                var currencyValue = Formatter.currencyEuro(response.averagePrice);

                this.currentAverage = response.averagePrice;

                $aggregateData
                    .html(`Средна цена за <strong>${response.propertyType.name}</strong> в <strong>${response.district.name}</strong> - <strong>${currencyValue}</strong>`);

                this.determinePriceDiff(this.$priceInput.val());
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