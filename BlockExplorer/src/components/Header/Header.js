import React, {Component} from 'react';
import './Header.css';
import {Link} from 'react-router-dom';


class Header extends Component {
    constructor(props) {
        super(props);
        this.state = {isToggleOn: false};

        this.handleClick = this.handleClick.bind(this);
    }

    handleClick() {
        this.setState(prevState => ({
            isToggleOn: !prevState.isToggleOn
        }));
    }

    render() {
        let menuActive = this.state.isToggleOn ? 'is-active' : '';

        return (
            <nav className="navbar is-info">
                <div className="navbar-brand">
                    <Link className="navbar-item" to='/'>
                        <img src="../../assets/blockchain-logo.png" alt="SoftUni blockchain test network" width="100%"
                             height="28"/>
                    </Link>
                    <div className={'navbar-burger burger ' + menuActive} data-target="navbar-main"
                         onClick={this.handleClick}>
                        <span></span>
                        <span></span>
                        <span></span>
                    </div>
                </div>

                <div id="navbar-main" className={'navbar-menu ' + menuActive}>
                    <div className="navbar-start">
                        <Link to="/" className="navbar-item r-item">Home</Link>
                        <Link to="/blocks" className="navbar-item r-item">Blocks</Link>
                    </div>

                    <div className="navbar-end">
                        <div className="navbar-item">
                            <div className="field is-grouped">
                            </div>
                        </div>
                    </div>
                </div>
            </nav>
        );
    }
}

export default Header;
