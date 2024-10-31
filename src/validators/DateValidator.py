from prompt_toolkit.validation import Validator, ValidationError


class DateValidator(Validator):
    def validate(self, document):
        text = document.text
        i = 0

        # Get index of first non-numeric character.
        # We want to move the cursor here.
        for i, char in enumerate(text):
            if char in '/0123456789':
                break

            raise ValidationError(message='This input contains non-numeric characters',
                              cursor_position=i)

        if text and len(text.split("/")) < 3:
            raise ValidationError(message='This input is an invalid date and is missing a "/"')
        if text and len(text.split("/")) > 3:
            raise ValidationError(message='This input is an invalid date and has too many "/"s')

