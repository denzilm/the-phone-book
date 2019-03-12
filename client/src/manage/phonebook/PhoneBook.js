import React, { Component } from 'react';
import { Form, Field } from 'react-final-form';

import * as Validators from '../../common/validation';
import Input from '../../common/components/form/Input';
import TextArea from '../../common/components/form/TextArea';

class PhoneBook extends Component {
	onSubmit = values => {
		if (!this.props.match.params.id) {
			this.props.createPhoneBook(values)
		} else {
			this.props.updatePhoneBook(values);
		}
	};

	render() {
		return (
			<>
				<h3 className='pt-4 mb-4 display-4 lead'>
					{!this.props.match.params.id
						? 'Create New Phone Book'
						: `You are viewing: ${this.props.phoneBook.title}`}
				</h3>
				<Form
					onSubmit={this.onSubmit}
					initialValues={this.props.phoneBook}
					render={({ handleSubmit, submitting, pristine, invalid }) => (
						<form onSubmit={handleSubmit}>
							<Field
								name='title'
								placeholder='Title'
								component={Input}
								validate={Validators.required('Title')}
							/>
							<Field
								name='description'
								placeholder='Description'
								component={TextArea}
								validate={Validators.composeValidators(
									Validators.required('Description'),
									Validators.maxLength(100)
								)}
								rows={3}
							/>

							<div>
								<input
									type='submit'
									className='btn btn-primary btn-lg'
									disabled={submitting || pristine || invalid}
									value={!this.props.match.params.id ? 'Create' : 'Update'}
								/>
							</div>
						</form>
					)}
				/>
			</>
		);
	}
}

export default PhoneBook;
