import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import Modal from '../../common/components/utils/Modal';
import history from '../../common/utils/history';
import { deletePhoneBook } from '../actions';

class PhoneBookItem extends Component {
	state = { showModal: false };
	componentDidUpdate() {
		if (this.state.showModal) {
			window.$('#modal').modal('show');
		}
	}

	renderActions() {
		return (
			<>
				<button
					type='button'
					className='btn btn-primary'
					onClick={() => {
						window.$('#modal').modal('hide');
						this.props.deletePhoneBook(this.props.id);
					}}
				>
					Delete
				</button>
				<button type='button' className='btn btn-secondary' data-dismiss='modal'>
					Close
				</button>
			</>
		);
	}

	render() {
		return (
			<div
				className='card border-success mb-2 phone-book overflow-auto'
				onClick={() => history.push(`/manage/phonebook/${this.props.id}`)}
			>
				<div className='card-header d-flex justify-content-between'>
					<h2 className='h2'>{this.props.title}</h2>
					<button
						className='btn btn-danger align-self-center remove'
						onClick={evt => {
							evt.stopPropagation();
							this.setState({ showModal: true });
							//;
						}}
					>
						Remove
					</button>
				</div>
				<div className='card-body' style={{ maxHeight: '120px' }}>
					<div className='d-flex'>
						<p className='card-text lead text-normal' style={{ width: '90%' }}>
							{this.props.description}
						</p>
						<Link
							to={`/manage/phonebook/${this.props.id}`}
							className='btn btn-primary ml-auto align-self-center'
						>
							<FontAwesomeIcon icon='sign-in-alt' className='text-white' size='2x' />
						</Link>
					</div>
				</div>
				{this.state.showModal && (
					<Modal
						title='Delete Phone Book'
						content={`Are you sure you want to delete phone book: ${this.props.title}`}
						actions={this.renderActions()}
					/>
				)}
			</div>
		);
	}
}

export default connect(
	null,
	{ deletePhoneBook }
)(PhoneBookItem);
