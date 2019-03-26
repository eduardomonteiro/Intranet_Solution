<?php
/*
	@author dhtmlx.com
	@license GPL, see license.txt
*/
require_once("base_connector.php");

class CommonDataProcessor extends DataProcessor{
	protected function get_post_values($ids){
		if (isset($_GET['action'])){
			$data = array();
			if (isset($_POST["id"])){
				$dataset = array();
				foreach($_POST as $key=>$value)
					$dataset[$key] = ConnectorSecurity::filter($value);

				$data[$_POST["id"]] = $dataset;
			}
			else
				$data["dummy_id"] = $_POST;
			return $data;
		}
		return parent::get_post_values($ids);
	}
	
	protected function get_ids(){
		if (isset($_GET['action'])){
			if (isset($_POST["id"]))
				return array($_POST['id']);
			else
				return array("dummy_id");
		}
		return parent::get_ids();
	}
	
	protected function get_operation($rid){
		if (isset($_GET['action']))
			return $_GET['action'];
		return parent::get_operation($rid);
	}
	
	public function output_as_xml($results){
		if (isset($_GET['action'])){
			LogMaster::log("Edit operation finished",$results);
			ob_clean();
			$type = $results[0]->get_status();
			if ($type == "error" || $type == "invalid"){
				echo "false";
			} else if ($type=="insert"){
				echo "true\n".$results[0]->get_new_id();
			} else 
				echo "true";
		} else
			return parent::output_as_xml($results);
	}
};

/*! DataItem class for DataView component
**/
class CommonDataItem extends DataItem{
	/*! return self as XML string
	*/
	function to_xml(){
		if ($this->skip) return "";
		return $this->to_xml_start().$this->to_xml_end();
	}

	function to_xml_start(){
		$str="<item id='".$this->get_id()."' ";
		for ($i=0; $i < sizeof($this->config->text); $i++){ 
			$name=$this->config->text[$i]["name"];
			$str.=" ".$name."='".$this->xmlentities($this->data[$name])."'";
		}

		if ($this->userdata !== false)
			foreach ($this->userdata as $key => $value)
				$str.=" ".$key."='".$this->xmlentities($value)."'";

		return $str.">";
	}
}


/*! Connector class for DataView
**/
class DataConnector extends Connector{

	/*! constructor
		
		Here initilization of all Masters occurs, execution timer initialized
		@param res 
			db connection resource
		@param type
			string , which hold type of database ( MySQL or Postgre ), optional, instead of short DB name, full name of DataWrapper-based class can be provided
		@param item_type
			name of class, which will be used for item rendering, optional, DataItem will be used by default
		@param data_type
			name of class which will be used for dataprocessor calls handling, optional, DataProcessor class will be used by default. 
	*/	
	public function __construct($res,$type=false,$item_type=false,$data_type=false,$render_type=false){
		if (!$item_type) $item_type="CommonDataItem";
		if (!$data_type) $data_type="CommonDataProcessor";

		$this->sections = array();

		if (!$render_type) $render_type="RenderStrategy";
		parent::__construct($res,$type,$item_type,$data_type,$render_type);

	}

	protected $sections;
	public function add_section($name, $string){
		$this->sections[$name] = $string;
	}

	protected function parse_request_mode(){
		if (isset($_GET['action']) && $_