import React, {Component} from 'react';
import './Home.sass';
import axios from 'axios';
import Moment from 'react-moment';
import {Link} from 'react-router-dom';

class Home extends Component {
    constructor(props) {
        super(props);

        this.state = {
            blocks: [],
            now: new Date()
        };
    }

    componentDidMount() {
        axios.get('http://localhost:5000/api/blocks').then(res => {
            const blocks = res.data;
            this.setState({blocks});
        })
    }

    render() {
        return <div>
            <section className="section">
                <div className="container">
                    <h1 className="title">Latest Blocks</h1>
                    {this.state.blocks.length > 0 &&
                    <table className="table has-text-centered is-hoverable" width="100%">
                        <thead>

                        <tr>
                            <th><abbr title="Height"></abbr>Height</th>
                            <th><abbr title="Age"></abbr>Age</th>
                            <th><abbr title="Transactions"></abbr>Transactions</th>
                            <th><abbr title="Total Sent"></abbr>Total Sent</th>
                        </tr>
                        </thead>
                        <tbody>
                        {this.state.blocks.reverse().map(block =>
                            <tr>
                                <td><Link to={'/blocks/' + block.blockHash}>{block.index}</Link></td>
                                <td><Moment fromNow ago>{block.createdOn}</Moment></td>
                                <td>{block.transactions.length}</td>
                                <td>{block.blockHash}</td>
                            </tr>
                        )}
                        </tbody>
                    </table>}
                </div>
            </section>
        </div>;
    }
}

export default Home;
