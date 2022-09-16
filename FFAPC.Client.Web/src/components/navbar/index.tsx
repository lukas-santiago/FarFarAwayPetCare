import { RiMenuUnfoldFill } from "react-icons/ri";

export function Navbar() {
  return (
    <div className="flex justify-between p-2 bg-slate-400 text-2xl">
      <button onClick={() => true}>
        <RiMenuUnfoldFill />
      </button>
      <img src="https://picsum.photos/100/20" alt="logo" />
      <div className="flex">
        <ul className="flex gap-2">
          <li>item</li>
          <li>item</li>
          <li>item</li>
        </ul>
      </div>
    </div>
  );
}
function menuAction(): JSX.MouseEventHandler<HTMLButtonElement> | undefined {
  throw new Error("Function not implemented.");
}
