import {
    Collapse,
    Navbar,
    NavbarToggler,
    NavbarBrand,
    Nav,
    NavItem,
    NavLink,
    UncontrolledDropdown,
    DropdownToggle,
    DropdownMenu,
    DropdownItem,
    NavbarText, 
    Button,
    ButtonGroup
} from 'reactstrap'
import React, { useContext, useState } from 'react';
import menus from '../config/menu.js'
import { AGSContext } from '../helpers/common/agsContext.js'
import { locale_Options } from '../config/localization.js'
import { SetLocaleCookieInClient, GetLocalizedString } from '../helpers/common/localizationHelper.js'
import "../styles/navbar.css"

export default function NavigationBar() {
    const agsContext = useContext(AGSContext);
    let menuOptions = JSON.parse(JSON.stringify(menus));
    menuOptions = GetValidMenuOptions(menuOptions, agsContext.functionClaims);
    
    const [isOpen, setIsOpen] = useState(false);
    const toggle = () => setIsOpen(!isOpen);
 
    return (
        <Navbar className="ags-navbar" expand="md">
            <NavbarBrand className=""><a href="/">AGS</a></NavbarBrand>
            <NavbarToggler onClick={toggle} />
            <Collapse isOpen={isOpen} navbar>
                <Nav className="mr-auto" navbar>
                    {
                        menuOptions.map(x => {
                            return (
                                <NavigationItem menuOption={x} level={0} />
                            )
                        })
                    }
                </Nav>
                <LocaleButtonGroups />
                <NavbarText>
                    <a href="/auth/logout" class="">Logout</a>
                </NavbarText>
            </Collapse>
        </Navbar>
    )
}

function LocaleButtonGroups(){
    const agsContext = useContext(AGSContext);
    
    function OnClickLocaleButton(e, locale){
        e.preventDefault();
        SetLocaleCookieInClient(locale)
        location.reload();
    }
    
    return (
        <div className="divLocale">
            {
                locale_Options.map((x, index) => {
                    const seperator = index != locale_Options.length - 1 ? "/" : "";
                    return (
                        <React.Fragment>
                            <a className={ agsContext.locale == x.value ? "locale-active" : "locale-inactive" } onClick={(e) => { OnClickLocaleButton(e, x.value)}}>{x.label}</a>
                            <span>{seperator}</span>
                        </React.Fragment>
                        // <Button color="primary" active={agsContext.locale == x.value} onClick={() => { OnClickLocaleButton(x.value) }} className="ags-link"></Button>
                    )
                })
            }
        </div>
    )
}

function NavigationItem({ menuOption, level }) {
    if (menuOption.ChildrenMenus == undefined || menuOption.ChildrenMenus.length == 0) {
        if (level == 0) {
            // this is the toppest level
            return (
                <NavItem>
                    <NavLink><p class="">{GetLocalizedString(menuOption.LabelKey)}</p></NavLink>
                </NavItem>
            )
        } else {
            // this is the lowest level
            return (
                <DropdownItem><a class="" href={menuOption.Url}>{GetLocalizedString(menuOption.LabelKey)}</a></DropdownItem>
            )
        }
    }
 
    if (menuOption.ChildrenMenus != undefined && menuOption.ChildrenMenus.length > 0) {
        // this is middle level and continue literation
        return (
            <UncontrolledDropdown inNavbar direction={level == 0 ? "down" : "right"}>
                <DropdownToggle nav caret className="">
                    {GetLocalizedString(menuOption.LabelKey)} 
                </DropdownToggle>
                <DropdownMenu className="">
                    {
                        menuOption.ChildrenMenus.map(x => {
                            return (
                                <NavigationItem level={level + 1} menuOption={x} />
                            )
                        })
                    }
                </DropdownMenu>
            </UncontrolledDropdown>
        )
    }
}
 
function GetValidMenuOptions(menuOptions, functionClaims){
    const validMenuOptions = [];
    menuOptions.forEach(x => {
        if (x.FunctionClaim == "" || functionClaims.some(y => y == x.FunctionClaim)){
            validMenuOptions.push(x);
        }
    });

    validMenuOptions.forEach(x => {
        x.ChildrenMenus = GetValidMenuOptions(x.ChildrenMenus, functionClaims);
    })

    return validMenuOptions;
}

