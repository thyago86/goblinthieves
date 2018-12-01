using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Jogador_Info : MonoBehaviour {

	

	/* Objetos Externos */
		public Grid currentGrid;
		public Jogador_Ataque PlayerAtk;
		public Jogador_Movimento PlayerMov;
		public Jogador_Interact PlayrInt;
	/* Objetos Externos */

	/* Prefabs */
		public GameObject ArmadilhaPrefab;
	/* Prefabs */


	/* Variaveis */
		public Vector3Int FacingTile;
		public string SelectedItem;
	/* Variaveis */
	
	/* Inventario */
		private List<string> CurrentItems = new List<string>();
		public Dictionary<string,int> Inventario_Count = new Dictionary<string, int>();
	/* Inventario */

	public void PegarItem(string Item){
		if(CurrentItems.Contains(Item)){
			Inventario_Count[Item]++;
			print(Inventario_Count[Item]);
		}else{
			Inventario_Count.Add(Item,1);
			CurrentItems.Add(Item);
		}
		print(Item +" : "+Inventario_Count[Item]);

	}
	public void UsarItem(string Item){
		if(Inventario_Count[Item]>0){

			switch(Item){
				case "Armadilha":
					GameObject obj = Instantiate(ArmadilhaPrefab,currentGrid.GetCellCenterWorld(FacingTile),Quaternion.identity);
					obj.GetComponent<ArmadilhaInteract>().Active = true;
				break;
			}

			Inventario_Count[Item]--;
			Mathf.Clamp(Inventario_Count[Item],0,Inventario_Count[Item]);
			if(Inventario_Count[Item] == 0){
				CurrentItems.Remove(Item);
			}
			print(Item +" : "+Inventario_Count[Item]);
		}
	}
	public void CycleItems(){
		int itemIndex = CurrentItems.IndexOf(SelectedItem);
		if(itemIndex < CurrentItems.Count){
			itemIndex++;
		}else{
			itemIndex = 0;
		}
			SelectedItem = CurrentItems[itemIndex];
	}

	void Start () {
		PlayerAtk = transform.GetComponent<Jogador_Ataque>();
		PlayerMov = transform.GetComponent<Jogador_Movimento>();
		PlayrInt = transform.GetComponent<Jogador_Interact>();
	}
	
	
	void Update () {
		if(!PlayerMov.Moving){
			if(Input.GetKeyDown(KeyCode.LeftShift)){
				CycleItems();
			}else if(Input.GetKeyDown(KeyCode.W)){
				UsarItem(SelectedItem);
			}
		}
		
	}
}
