import { useReducer, useState } from "preact/hooks";
import "./app.css";
import { Navbar } from "./components/navbar";
import { Sidebar } from "./components/sidebar";

const sideBarInitialState: string[] = [];
const sideBarStateReducer = (state: string[], action: string) => {
  console.log(action, state);

  switch (action) {
    case "show":
      return [onVisibibleClass, onAnimationClass];
    case "transitionEnd":
      let index = state.indexOf(onAnimationClass);
      if (index !== -1) state.splice(index, 1);
      return state;
    case "hide":
      return [onAnimationClass];
    default:
      throw new Error("Unknown action type: " + action.type + " on reducer");
  }
};
const onAnimationClass = "sidebar-container--animatable";
const onVisibibleClass = "sidebar-container--visible";

export function App() {
  const [sideBarState, sideBarStateDispatch] = useReducer(sideBarStateReducer, sideBarInitialState);

  const toggleSideBarVisibility = () => {
    if (sideBarState.includes(onVisibibleClass)) sideBarStateDispatch("hide");
    else sideBarStateDispatch("show");
  };
  const transitionEndHandler = () => {
    sideBarStateDispatch("transitionEnd");
  };

  return (
    <>
      {/* <Sidebar /> */}
      <div class={`sidebar-container ${sideBarState.join(" ")}`} onClick={toggleSideBarVisibility}>
        <div class="sidebar" onTransitionEnd={transitionEndHandler}>
          <img src="https://picsum.photos/100/20" alt="logo" />
          <ul>
            <li>Lorem</li>
            <li>Lorem</li>
            <li>Lorem</li>
            <li>Lorem</li>
            <li>Lorem</li>
            <li>Lorem</li>
          </ul>
          <ul>
            <li>Lorem</li>
            <li>Lorem</li>
          </ul>
        </div>
      </div>
      <div class="flex flex-col">
        {/* <Navbar /> */}
        <div class="bg-slate-300">
          <div class="bg-slate-400 h-10 w-10" onClick={toggleSideBarVisibility}></div>
        </div>
        Content!
      </div>
    </>
  );
}
