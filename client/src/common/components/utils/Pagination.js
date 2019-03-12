import React from 'react';
import { Link } from 'react-router-dom';
import classnames from 'classnames';
import _ from 'lodash';

export default ({ url, pagingInfo }) => {
	return (
		<div className='btn-group mt-2'>
			{_.range(pagingInfo.totalPages).map(num => (
				<Link
					to={`${url.substring(0, url.length)}/page${num +
						1}`}
					key={num + 1}
					className={classnames('btn btn-primary', {
						active: num + 1 === pagingInfo.currentPage
					})}
				>
					{num + 1}
				</Link>
			))}
		</div>
	);
};
